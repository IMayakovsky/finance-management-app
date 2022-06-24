using FinanceManagement.Caching.Dependencies;

namespace FinanceManagement.Core.Caching.Invalidation
{
    public class DataCacheNotification
    {
        public CacheDependencyType DataType { get; set; }
        public int[] DataId { get; set; }
    }
}
