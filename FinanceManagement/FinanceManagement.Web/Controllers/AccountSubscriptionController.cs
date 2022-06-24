using FinanceManagement.Infrastructure.Attributes;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManagement.Web.Controllers
{
    /// <summary>
    /// Controller for Subscriptions Operations
    /// </summary>
    [BaseAuthorize]
    [AccountAccess]
    [Route("api/accounts/{accountId}/subscriptions")]
    public class AccountSubscriptionController : BaseController
    {
        private readonly Lazy<ISubscriptionOperation> subscriptionOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subscriptionOperation"></param>
        public AccountSubscriptionController(Lazy<ISubscriptionOperation> subscriptionOperation)
        {
            this.subscriptionOperation = subscriptionOperation;
        }

        /// <summary>
        /// Returns Account's Subscriptions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<SubscriptionDto>> GetAccountSubscriptions(int accountId)
        {
            return await subscriptionOperation.Value.GetAccountSubscriptionsCached(new List<int> { accountId });
        }

        /// <summary>
        /// Deletes subscriptions
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        [HttpDelete("{subscriptionId}")]
        public async Task DeleteSubscription(int accountId, int subscriptionId)
        {
            await subscriptionOperation.Value.DeleteSubscription(subscriptionId, accountId);
        }

        /// <summary>
        /// Creates Subscription
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="subscription"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SubscriptionDto> CreateSubscription(int accountId, SubscriptionDto subscription)
        {
            subscription.AccountId = accountId;
            subscription.UserId = userId;

            return await subscriptionOperation.Value.CreateSubscription(subscription);
        }

        /// <summary>
        /// Update Subscription
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="subscriptionId"></param>
        /// <param name="subscription"></param>
        /// <returns></returns>
        [HttpPut("{subscriptionId}")]
        public async Task UpdateSubscription(int accountId, int subscriptionId, SubscriptionDto subscription)
        {
            subscription.Id = subscriptionId;

            await subscriptionOperation.Value.UpdateSubscription(accountId, userId, subscription);
        }
    }
}
