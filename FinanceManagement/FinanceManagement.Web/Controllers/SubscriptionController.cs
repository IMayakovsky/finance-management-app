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
    [Route("api/subscriptions")]
    public class SubscriptionController : BaseController
    {
        private readonly Lazy<ISubscriptionOperation> subscriptionOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subscriptionOperation"></param>
        public SubscriptionController(Lazy<ISubscriptionOperation> subscriptionOperation)
        {
            this.subscriptionOperation = subscriptionOperation;
        }

        /// <summary>
        /// Returns Account's Subscriptions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<SubscriptionDto>> GetUserSubscriptions()
        {
            return await subscriptionOperation.Value.GetUserSubscriptionsCached(userId);
        }
    }
}
