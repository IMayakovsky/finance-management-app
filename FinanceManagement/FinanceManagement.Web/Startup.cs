using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using FinanceManagement.Infrastructure.Ioc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using FinanceManagement.Infrastructure.Configuration;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VueCliMiddleware;
using Microsoft.AspNetCore.SpaServices;
using FinanceManagement.Core.Caching.CacheModules;
using StackExchange.Profiling.Storage;
using FinanceManagement.Infrastructure.Common.Constants;
using FinanceManagement.Infrastructure.Extensions;
using FinanceManagement.Infrastructure.Helpers;
using StackExchange.Profiling;
using Serilog;
using FinanceManagement.Core.Logging.Factory;
using FinanceManagement.Infrastructure.Hubs;
using FinanceManagement.Infrastructure.Hubs.Constants;

namespace Finance_Management
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                    });
            });

            services.AddSwaggerGen(options =>
            {
                //Documentation: https://github.com/domaindrivendev/Swashbuckle.AspNetCore

                var webXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, webXmlFile);

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {access_token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
                options.IgnoreObsoleteActions();
                options.CustomSchemaIds(type => type.ToString());
            });

            services.AddSingleton(new LoggerFactory(Configuration, Environment.EnvironmentName));

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = ProfilerConstants.ProfilerRoute;
                options.UserIdProvider = request => request.HttpContext.GetUserId();
                options.ShouldProfile = request => RequestLogHelper.ShouldBeLoggedOrProfiled(request.Path.Value);
            }).AddEntityFramework();

            new InfrastructureServiceInstaller().InstallServices(Configuration, services, options => options.UseNpgsql(Configuration.GetConnectionString("FinanceManagementDbContext")));

            services.AddHostedService(p => p.GetRequiredService<CachePreloadingService>());

            ConfigureAuthServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                app.UseCors();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Finance Management API";

                c.DisplayRequestDuration();

                // we are using custom index html page for Swashbuckle, copied from Swashbuckle git repo:
                // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/src/Swashbuckle.AspNetCore.SwaggerUI/index.html
                // with a small modification to read authorization token from localStorage for developers convenience
                // in case of updating this file, look into the current swashbuckle.html and find 'Newton media'
                c.IndexStream = () => File.OpenRead("wwwroot/swashbuckle.html");

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finance Management API");

                c.DefaultModelsExpandDepth(-1);
            });

            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiniProfiler();

            app.Use(async (context, next) =>
            {
                if (MiniProfiler.Current != null)
                {
                    context.Items["ProfilerId"] = MiniProfiler.Current.Id;
                }

                await next.Invoke();
            });

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("UserId", httpContext.GetUserId());

                    if (httpContext.Items.ContainsKey("ProfilerId"))
                    {
                        diagnosticContext.Set("ProfilerUrl", ProfilerConstants.GetUrlToProfilationDetail((Guid)httpContext.Items["ProfilerId"]));
                    }
                };
            });

            new InfrastructureServiceInstaller().ConfigureAfterInstall(app.ApplicationServices);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
#if !DEBUG_BACKEND_ONLY && !RELEASE_BACKEND_ONLY
                endpoints.MapToVueCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "ClientApp" },
#if RELEASE
                    npmScript: null,
#endif
                    forceKill: true,
                    wsl: false,
                    port: 8081
                );
#endif
                endpoints.MapHub<NotificationHub>(HubRoutes.NotificationHub);
            });
        }

        private void ConfigureAuthServices(IServiceCollection services)
        {
            //The application which uses Newton.Core.Auth must configure TokenValidationParameters

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = Configuration["AppConfiguration:TokenConfiguration:JwtTokenAudience"],
                ValidIssuer = Configuration["AppConfiguration:TokenConfiguration:JwtTokenIssuer"],
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppConfiguration:TokenConfiguration:JwtIssuerSigningKey"])),
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };

            new AuthConfigurator(tokenValidationParameters).ConfigureServices(services);
        }
    }
}
