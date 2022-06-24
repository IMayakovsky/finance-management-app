using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Caching.CacheModules;
using FinanceManagement.Core.Caching.Invalidation;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.CacheModules;

public class AccountCacheModule : CacheModule
{
    public AccountCacheModule(IMemoryCache cache, IServiceScopeFactory scopeFactory) : base(cache, scopeFactory)
    {
    }

    public override bool HandleDataCacheNotification(DataCacheNotification notification)
    {
        if (notification.DataType != CacheDependencyType.AccountLastTransaction || notification.DataType != CacheDependencyType.AccountSubscription) return false;

        AddPreloadRequest(RequestPriority.HighPriority, new AccountItemsCachePreloadRequest()
        {
            AccountId = notification.DataId[0],
            Type = notification.DataType
        });

        return true;
    }

    protected override async Task ProcessPreloadRequest(CachePreloadRequest request)
    {
        if (request is not AccountItemsCachePreloadRequest lastTransactionsReload)
        {
            return;
        }
        using (var scope = scopeFactory.CreateScope())
        {
            if (lastTransactionsReload.Type == CacheDependencyType.AccountLastTransaction)
            {
                var transactionOperation = scope.ServiceProvider.GetService<ITransactionOperation>();
                await transactionOperation.GetLastAccountTransactionsCached(lastTransactionsReload.AccountId);
            } 
            else if (lastTransactionsReload.Type == CacheDependencyType.AccountSubscription)
            {
                var subscriptionOperation = scope.ServiceProvider.GetService<ISubscriptionOperation>();
                await subscriptionOperation.GetAccountSubscriptionsCached(new List<int> { lastTransactionsReload.AccountId }, true);
            }
        }
    }

    public override Task InitAsync()
    {
        // Account depends on user and has preload in UserCacheModule

        return Task.CompletedTask;
    }

    protected override int PreloadThreadsCount => 5;
}

public class AccountItemsCachePreloadRequest : CachePreloadRequest
{
    public int AccountId { get; set; }
    public CacheDependencyType Type { get; set; }
}

