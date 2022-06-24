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

public class GroupCacheModule : CacheModule
{
    public GroupCacheModule(IMemoryCache cache, IServiceScopeFactory scopeFactory) : base(cache, scopeFactory)
    {
    }

    public override bool HandleDataCacheNotification(DataCacheNotification notification)
    {
        if (notification.DataType != CacheDependencyType.Group) return false;

        AddPreloadRequest(RequestPriority.HighPriority, new GroupCachePreloadRequest()
        {
            GroupIds = notification.DataId,
        });

        return true;
    }

    public override async Task InitAsync()
    {
        await PreloadAllGroups(true);
    }

    private async Task PreloadAllGroups(bool isFromCacheModuleInitialization)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var groupOperation = scope.ServiceProvider.GetService<IGroupOperation>();

            int skip = 0;
            const int dbPageSize = 1000;
            const int requestBatchSize = 100;

            while (true)
            {
                IEnumerable<int> userIds = await groupOperation.GetGroupIds(dbPageSize, skip);

                skip += dbPageSize;

                if (!userIds.Any())
                {
                    break;
                }

                foreach (var userIdsBatch in userIds.Batch(requestBatchSize))
                {
                    AddPreloadRequest(RequestPriority.LowPriority, new GroupCachePreloadRequest()
                    {
                        GroupIds = userIdsBatch,
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
            if (request is GroupCachePreloadRequest groupReload)
            {
                var userOperation = scope.ServiceProvider.GetService<IGroupOperation>();
                await userOperation.PreloadGroups(groupReload.GroupIds.ToList());
            }
        }
    }

    protected override int PreloadThreadsCount => 5;
}

public class GroupCachePreloadRequest : CachePreloadRequest
{
    public IEnumerable<int> GroupIds { get; set; }
}

