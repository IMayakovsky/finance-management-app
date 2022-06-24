using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Caching.CacheModules;
using FinanceManagement.Core.Caching.Invalidation;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.CacheModules;

public class UserCacheModule : CacheModule
{
    public UserCacheModule(IMemoryCache cache, IServiceScopeFactory scopeFactory) : base(cache, scopeFactory)
    {
    }

    public override bool HandleDataCacheNotification(DataCacheNotification notification)
    {
        if (notification.DataType < CacheDependencyType.User || notification.DataType > CacheDependencyType.UserGroupRole) return false;

        if (notification.DataType == CacheDependencyType.User)
        {
            AddPreloadRequest(RequestPriority.HighPriority, new UserCachePreloadRequest()
            {
                UserIds = notification.DataId,
            });
        }
        else
        {
            AddPreloadRequest(RequestPriority.HighPriority, new UserDataPreloadRequest()
            {
                UserId = notification.DataId[0],
                DataType = notification.DataType,
            });
        }

        return true;
    }

    public override async Task InitAsync()
    {
        await PreloadAllUsers(true);
    }

    private async Task PreloadAllUsers(bool isFromCacheModuleInitialization)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var userOperation = scope.ServiceProvider.GetService<IUserOperation>();

            int skip = 0;
            const int dbPageSize = 1000;
            const int requestBatchSize = 100;

            while (true)
            {
                IEnumerable<int> userIds = await userOperation.GetUserIds(dbPageSize, skip);

                skip += dbPageSize;

                if (!userIds.Any())
                {
                    break;
                }

                foreach (var userIdsBatch in userIds.Batch(requestBatchSize))
                {
                    AddPreloadRequest(RequestPriority.LowPriority, new UserCachePreloadRequest()
                    {
                        UserIds = userIdsBatch,
                        IsFromCacheModuleInitialization = isFromCacheModuleInitialization,
                    });
                }
            }
        }
    }

    protected override async Task ProcessPreloadRequest(CachePreloadRequest request)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            if (request is UserCachePreloadRequest userReload)
            {
                var userOperation = scope.ServiceProvider.GetService<IUserOperation>();
                await userOperation.PreloadUsers(userReload.UserIds.ToList(), userReload.OnlyUserModelData);
            }
            else if (request is UserDataPreloadRequest userDataReload)
            {
                switch (userDataReload.DataType)
                {
                    case CacheDependencyType.UserGoal:
                        var goalOperation = scope.ServiceProvider.GetService<IGoalOperation>();
                        await goalOperation.GetUserGoalsCached(userDataReload.UserId, true);
                        break;
                    case CacheDependencyType.UserDebt:
                        var debtOperation = scope.ServiceProvider.GetService<IDebtOperation>();
                        await debtOperation.GetUserDebtsCached(userDataReload.UserId, true);
                        break;
                    case CacheDependencyType.UserGroupRole:
                        var groupOperation = scope.ServiceProvider.GetService<IGroupOperation>();
                        await groupOperation.GetUserGroupsRolesCached(userDataReload.UserId, false, true);
                        break;
                    case CacheDependencyType.UserAccount:
                        var accountOperation = scope.ServiceProvider.GetService<IAccountOperation>();
                        await accountOperation.GetUserAccountsCached(userDataReload.UserId, true, true);
                        break;
                    case CacheDependencyType.UserCategory:
                        var transactionOperation = scope.ServiceProvider.GetService<ITransactionOperation>();
                        await transactionOperation.GetUserCategoriesCached(userDataReload.UserId, true);
                        break;
                }
            }
        }
    }

    protected override int PreloadThreadsCount => 5;
}

public class UserCachePreloadRequest : CachePreloadRequest
{
    public IEnumerable<int> UserIds { get; set; }
    public bool OnlyUserModelData { get; set; }
}

public class UserDataPreloadRequest : CachePreloadRequest
{
    public int UserId { get; set; }
    public CacheDependencyType DataType { get; set; }
}

