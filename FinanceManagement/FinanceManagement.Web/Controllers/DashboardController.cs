using FinanceManagement.Infrastructure.Attributes;
using FinanceManagement.Infrastructure.Common.Constants;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Dashboards;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManagement.Web.Controllers
{
    /// <summary>
    /// Controller for Dashboard Operations
    /// </summary>
    [BaseAuthorize]
    [Route("api/dashboard/accounts/{accountId}")]
    public class DashboardController : BaseController
    {
        private readonly Lazy<IDashboardOperation> dashboardOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dashboardOperation"></param>
        public DashboardController(Lazy<IDashboardOperation> dashboardOperation)
        {
            this.dashboardOperation = dashboardOperation;
        }

        /// <summary>
        /// Returns Top User Entities
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("init")]
        public async Task<DashboardDataDto> GetTopEntities(int accountId)
        {
            return await dashboardOperation.Value.GetTopEntities(userId, accountId, DashboardConstants.TopDataCount);
        }
    }
}
