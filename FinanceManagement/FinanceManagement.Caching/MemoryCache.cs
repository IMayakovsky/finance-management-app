using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Caching.CacheModules;
using FinanceManagement.Core.Caching.Invalidation;

namespace FinanceManagement.Caching
{
    public interface IMemoryCache
    {
        ICacheProvider Provider { get; }
        ICacheDependencyManager Dependencies { get; }
        bool Enabled { get; set; }
        void Invalidate(DataCacheNotification invalidateMessage);
    }

    public class MemoryCache : IMemoryCache
    {
        public ICacheProvider Provider => cacheProvider;
        public ICacheDependencyManager Dependencies => dependencies;
        public bool Enabled { get; set; }

        private readonly ICacheProvider cacheProvider;
        private readonly ICacheDependencyManager dependencies;
        private readonly CachePreloadingService cachePreloadingService;

        public MemoryCache(ICacheProvider cacheProvider, ICacheDependencyManager dependencies, CachePreloadingService cachePreloadingService)
        {
            this.cacheProvider = cacheProvider;
            this.dependencies = dependencies;
            this.cachePreloadingService = cachePreloadingService;
        }

        public void Invalidate(DataCacheNotification invalidateMessage)
        {
            Cache.Current.Dependencies.InvalidateByDependencies(invalidateMessage.DataType, invalidateMessage.DataId);
            cachePreloadingService.HandleDataCacheNotification(invalidateMessage);
        }
    }
}
