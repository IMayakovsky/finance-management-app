using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FinanceManagement.Caching
{
    public interface ICacheProvider
    {
        CacheValue<T> Get<T>(string cacheKey);
        void Set<T>(string cacheKey, T value);
        void RemoveAll(IEnumerable<string> cacheKeys);
    }

    public class CacheProvider : ICacheProvider
    {
        private readonly ConcurrentDictionary<string, CacheValue<object>> memoryCache = new ConcurrentDictionary<string, CacheValue<object>>();

        public CacheValue<T> Get<T>(string cacheKey)
        {
            if (!memoryCache.ContainsKey(cacheKey))
            {
                return CacheValue<T>.NoValue;
            }

            if (memoryCache[cacheKey].Value is T)
            {
                return new CacheValue<T>((T)memoryCache[cacheKey].Value, true);
            }

            return CacheValue<T>.Null;
        }

        public void Set<T>(string cacheKey, T value)
        {
            memoryCache[cacheKey] = new CacheValue<object>(value, true);
        }

        public void RemoveAll(IEnumerable<string> cacheKeys)
        {
            foreach (string key in cacheKeys)
            {
                memoryCache.Remove(key, out _);
            }
        }
    }
}
