using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Caching.CacheModules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManagement.Caching.Ioc
{
    public interface ICacheServiceInstaller
    {
        void InstallServices(IConfiguration configuration, IServiceCollection services);
    }

    public class CacheServiceInstaller : ICacheServiceInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<ICacheDependencyManager, CacheDependencyManager>();
            services.AddSingleton<ICacheProvider, CacheProvider>();
            services.AddSingleton<IMemoryCache, MemoryCache>();

            services.AddSingleton<CachePreloadingService>();
        }
    }
}
