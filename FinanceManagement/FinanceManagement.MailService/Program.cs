using FinanceManagement.Infrastructure.Ioc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Configuration;

namespace FinanceManagement.MessageService
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            IServiceCollection services = new ServiceCollection().AddLogging();

            new InfrastructureServiceInstaller().InstallServices(configuration, services, options => options.UseNpgsql(configuration.GetConnectionString("FinanceManagementDbContext")));
            services.AddSingleton<IMailSender>(new MailSender(configuration));

            new MessageServiceProcess(services).Process().Wait();
        }
    }
}
