using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Caching.CacheModules;
using FinanceManagement.Infrastructure.Dto;
using Xunit;

namespace FinanceManagement.Caching.Test
{
    public class CacheTest
    {
        private const string TEST_CACHE_KEY = "test_key";
        private const string TEST_CACHE_VALUE = "test_value";

        private readonly ICacheProvider provider;
        private readonly CachePreloadingService cachePreloader;

        public CacheTest()
        {
            provider = new CacheProvider();
            cachePreloader = new CachePreloadingService();
            Cache.Initialize(new MemoryCache(provider, new CacheDependencyManager(provider), cachePreloader));
        }

        [Fact]
        public void SetAndGet_Comparing_Success()
        {
            provider.Set(TEST_CACHE_KEY, TEST_CACHE_VALUE);

            var result = provider.Get<string>(TEST_CACHE_KEY);

            Assert.True(result.HasValue);

            Assert.Equal(TEST_CACHE_VALUE, result.Value);
        }

        [Fact]
        public void SetGetGet_Changing_Changed()
        {
            int userIdBefore = 1, userIdAfter = 2;

            UserDto user = new UserDto { Id = userIdBefore };

            provider.Set(TEST_CACHE_KEY, user);

            var result = provider.Get<UserDto>(TEST_CACHE_KEY);

            Assert.True(result.HasValue);

            result.Value.Id = userIdAfter;

            result = provider.Get<UserDto>(TEST_CACHE_KEY);

            Assert.True(result.HasValue);

            Assert.Equal(userIdAfter, result.Value.Id);
        }
    }
}
