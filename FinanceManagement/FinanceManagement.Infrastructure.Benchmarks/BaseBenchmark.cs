using FinanceManagement.Infrastructure.Ioc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManagement.Infrastructure.Benchmarks
{
    public class BaseBenchmark
    {
        protected ServiceProvider Services { get; private set; }
        protected IConfiguration Configuration { get; private set; }

        public BaseBenchmark()
        {
            SetupServices();
        }

        public void SetupServices()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);

            var builtAppSettings = builder.Build();

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddLogging();

            new InfrastructureServiceInstaller().InstallServices(Configuration, services, options => options.UseNpgsql(Configuration.GetConnectionString("FinanceManagementDbContext")));

            Services = services.BuildServiceProvider();

            new InfrastructureServiceInstaller().ConfigureAfterInstall(Services);
        }
    }
}
