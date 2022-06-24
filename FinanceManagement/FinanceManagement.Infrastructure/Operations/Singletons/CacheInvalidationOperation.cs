using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Caching.Invalidation;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Operations.Base;

namespace FinanceManagement.Infrastructure.Operations.Singletons
{
    public interface ICacheInvalidationOperation : ISingletonOperation
    {
        void Invalidate(CacheDependencyType type, params int[] ids);
    }

    public class CacheInvalidationOperation : BaseInfrastructureOperation, ICacheInvalidationOperation
    {
        private readonly ICacheDependencyManager dependencies;
        private readonly IMemoryCache cache;

        public CacheInvalidationOperation(ICacheDependencyManager dependencies, IMemoryCache cache)
        {
            this.dependencies = dependencies;
            this.cache = cache;
        }

        public void Invalidate(CacheDependencyType type, params int[] ids)
        {
            var notification = new DataCacheNotification()
            {
                DataType = type,
                DataId = ids,
            };

            cache.Invalidate(notification);
        }
    }
}
