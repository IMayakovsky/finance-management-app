using FinanceManagement.Caching;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FinanceManagement.Infrastructure.Test.Common
{
    public class CacheTest : BaseUnitTest
    {
        [Fact]
        public async Task GetUserByIdCachedTest()
        {
            int userId = 1;
            var cachekey = Cache.GetKey(typeof(UserDto), userId);
            var userOperation = Services.GetService<IUserOperation>();

            Cache.Current.Provider.RemoveAll(new List<string> { cachekey }); //clear cache

            var user = await userOperation.GetUserByIdCached(userId);

            Assert.True(user != null); // user found

            var cacheItem = Cache.Current.Provider.Get<UserDto>(cachekey);

            Assert.True(cacheItem.HasValue && !cacheItem.IsNull); // cache Item exists
            Assert.Equal(user.Id, cacheItem.Value.Id); // cache Item is the same as function result Item
        }

        [Fact]
        public async Task GetUserAccountsCachedTest()
        {
            int userId = 1;
            var cachekey = Cache.GetKey(typeof(AccountDto), typeof(UserDto), userId);
            var accountOperation = Services.GetService<IAccountOperation>();

            Cache.Current.Provider.RemoveAll(new List<string> { cachekey }); // clear cache

            var userAccounts = await accountOperation.GetUserAccountsCached(userId);
            var cacheItem = Cache.Current.Provider.Get<List<AccountDto>>(cachekey);

            Assert.True(cacheItem.HasValue && !cacheItem.IsNull); // cache Item exists
            Assert.Equal(userAccounts.Count, cacheItem.Value.Count); // cache Item is the same as function result Item
        }
    }
}
