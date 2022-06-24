using Dawn;
using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Helpers;
using FinanceManagement.Infrastructure.Mappers;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Base;
using FinanceManagement.Infrastructure.Operations.Singletons;
using FinanceManagement.Infrastructure.Repositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Operations.Transients
{
    public interface ISubscriptionOperation : ITransientOperation
    {
        Task<List<SubscriptionDto>> GetAccountSubscriptionsCached(List<int> accountIds, bool ignoreCachedValue = false);
        Task<List<SubscriptionDto>> GetUserSubscriptionsCached(int userId);
        Task<List<SubscriptionDto>> GetExpiredSubscriptions(int remainingDays);
        Task<SubscriptionDto> CreateSubscription(SubscriptionDto subscription);
        Task UpdateSubscription(int accountId, int userId, SubscriptionDto subscription);
        Task DeleteSubscription(int subscriptionId, int accountId);
        Task<List<SubscriptionDto>> GetSubscriptionsWithoutActualBill();
    }

    public class SubscriptionOperation : BaseInfrastructureOperation, ISubscriptionOperation
    {
        private readonly IDataAccess dataAccess;
        private readonly ICacheInvalidationOperation cacheInvalidationOperation;
        private readonly Lazy<ISecurityHelper> securityHelper;
        private readonly Lazy<IAccountOperation> accountOperation;

        public SubscriptionOperation(IDataAccess dataAccess, ICacheInvalidationOperation cacheInvalidationOperation, Lazy<ISecurityHelper> securityHelper, Lazy<IAccountOperation> accountOperation)
        {
            this.dataAccess = dataAccess;
            this.cacheInvalidationOperation = cacheInvalidationOperation;
            this.securityHelper = securityHelper;
            this.accountOperation = accountOperation;
        }

        public async Task<List<SubscriptionDto>> GetAccountSubscriptionsCached(List<int> accountIds, bool ignoreCachedValue = false)
        {
            var cached = await this.ExecuteMemoryCachedMultipleAsync(accountIds,
                id => Cache.GetKey(typeof(AccountSubscriptionsDto), id),
                sub => sub.AccountId,
                GetSubscriptionsByAccountId,
                sub => new[] { new CacheDependency(CacheDependencyType.AccountSubscription, sub.AccountId) },
                ignoreCachedValue);

            return cached.SelectMany(e => e.Subscriptions).ToList();
        }

        public async Task<List<SubscriptionDto>> GetUserSubscriptionsCached(int userId)
        {
            var accounts = await accountOperation.Value.GetUserAccountsCached(userId, true);

            return await GetAccountSubscriptionsCached(accounts.Select(a => a.Id).ToList());
        }

        public async Task<List<SubscriptionDto>> GetExpiredSubscriptions(int remainingDays)
        {
            var models =  await dataAccess.Repository<ISubscriptionRepository>().GetExpiredSubscriptionsWithAccount(DateTime.UtcNow + TimeSpan.FromDays(remainingDays));

            return models.Select(m => m.Map()).ToList();
        }

        public async Task<SubscriptionDto> CreateSubscription(SubscriptionDto subscription)
        {
            Guard.Argument(subscription, nameof(subscription)).NotNull();

            var model = subscription.Adapt<Subscription>();

            await dataAccess.Repository<ISubscriptionRepository>().InsertAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.AccountSubscription, model.AccountId);

            return model.Map(subscription.UserId);
        }

        public async Task UpdateSubscription(int accountId, int userId, SubscriptionDto subscription)
        {
            Guard.Argument(subscription, nameof(subscription)).NotNull();

            var model = await dataAccess.Repository<ISubscriptionRepository>().GetByIdAndAccountId(subscription.Id, accountId);

            if (model == null)
            {
                throw new NotFoundException();
            }

            if (subscription.AccountId != accountId)
            {
                await securityHelper.Value.CheckUserAccountPermissions(userId, subscription.AccountId);
            }

            model = subscription.Adapt<Subscription>();

            await dataAccess.Repository<ISubscriptionRepository>().UpdateAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.AccountSubscription, accountId);

            if (subscription.AccountId != accountId)
            {
                cacheInvalidationOperation.Invalidate(CacheDependencyType.AccountSubscription, subscription.AccountId);
            }
        }

        public async Task DeleteSubscription(int subscriptionId, int accountId)
        {
            var model = await dataAccess.Repository<ISubscriptionRepository>().GetByIdAndAccountId(subscriptionId, accountId);

            if (model == null)
            {
                throw new NotFoundException();
            }

            await dataAccess.Repository<ISubscriptionRepository>().RemoveAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.AccountSubscription, accountId);
        }

        public async Task<List<SubscriptionDto>> GetSubscriptionsWithoutActualBill()
        {
            var models = await dataAccess.Repository<ISubscriptionRepository>().GetWithoutActualBillWithAccount(DateTime.UtcNow);

            return models.Select(m => m.Map()).ToList();
        }

        private async Task<List<AccountSubscriptionsDto>> GetSubscriptionsByAccountId(List<int> accountIds)
        {
            var groupped = (await dataAccess.Repository<ISubscriptionRepository>().GetByAccountIdsIncludesAccount(accountIds)).GroupBy(u => u.AccountId);

            var res = new List<AccountSubscriptionsDto>();

            foreach (var group in groupped)
            {
                res.Add(new AccountSubscriptionsDto
                {
                    AccountId = group.Key,
                    Subscriptions = group.Select(g => g.Map()).ToList()
                });
            }

            return res;
        }
    }
}
