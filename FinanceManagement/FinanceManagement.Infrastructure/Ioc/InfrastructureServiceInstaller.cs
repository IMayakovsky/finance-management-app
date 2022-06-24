using System;
using System.Text.Json.Serialization;
using FinanceManagement.Caching;
using FinanceManagement.Caching.Ioc;
using FinanceManagement.Core.Caching.CacheModules;
using FinanceManagement.Core.Helpers;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.CacheModules;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Extensions;
using FinanceManagement.Infrastructure.Mappers;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace FinanceManagement.Infrastructure.Ioc
{
    public interface IInfrastructureServiceInstaller
    {
        void InstallServices(IConfiguration configuration, IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions);

        void ConfigureAfterInstall(IServiceProvider provider);
    }
    public class InfrastructureServiceInstaller : IInfrastructureServiceInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions)
        {
            MapperConfiguration.Configure();

            services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));
            
            services.AddSingleton<IDbContextFactory<GeneratedDbContext>>((services) => new DbContextFactory(dbContextOptions)); 

            services.AddFactory<IDataAccess, DataAccess>();

            services.Scan(scan => scan
               .FromAssemblyOf<InfrastructureServiceInstaller>()

               .AddClasses(classes => classes.AssignableTo<ITransientOperation>())
               .AsImplementedInterfaces()
               .WithTransientLifetime()

               .AddClasses(classes => classes.AssignableTo<ISingletonOperation>())
               .AsImplementedInterfaces()
               .WithTransientLifetime()

               .AddClasses(classes => classes.AssignableTo<IRepository>())
                .AsImplementedInterfaces()
                .WithTransientLifetime()

                .AddClasses(classes => classes.AssignableTo<IHelper>())
                .AsImplementedInterfaces()
                .WithTransientLifetime()

                .AddClasses(classes => classes.AssignableTo<ICacheModule>())
                .AsSelf()
                .WithSingletonLifetime()
            );

            services.AddSignalR(opt =>
            {
                opt.EnableDetailedErrors = true;
            }).AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Converters
                    .Add(new JsonStringEnumConverter());
            });

            new CacheServiceInstaller().InstallServices(configuration, services);
        }

        public void ConfigureAfterInstall(IServiceProvider provider)
        {
            Cache.Initialize(provider.GetService<IMemoryCache>());

            ConfigureCacheModules(provider);
        }

        private void ConfigureCacheModules(IServiceProvider provider)
        {
            var cacheService = provider.GetService<CachePreloadingService>();

            // order matters - it is the order in which notification messages are processed
            cacheService.AddCacheModule(provider.GetService<UserCacheModule>());
            cacheService.AddCacheModule(provider.GetService<GroupCacheModule>());
            cacheService.AddCacheModule(provider.GetService<AccountCacheModule>());
        }
    }
}
