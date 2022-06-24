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
    /// Controller for Debts
    /// </summary>
    [BaseAuthorize]
    [Route("api/debts")]
    public class DebtController : BaseController
    {
        private readonly Lazy<IDebtOperation> debtOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="debtOperation"></param>
        public DebtController(Lazy<IDebtOperation> debtOperation)
        {
            this.debtOperation = debtOperation;
        }

        /// <summary>
        /// Returns Users Debts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DebtDto>> GetUserDebts()
        {
            return await debtOperation.Value.GetUserDebtsCached(userId);
        }

        /// <summary>
        /// Deletes Debt
        /// </summary>
        /// <param name="debtId"></param>
        /// <returns></returns>
        [HttpDelete("{debtId}")]
        public async Task DeleteDebt(int debtId)
        {
            await debtOperation.Value.DeleteDebt(debtId, userId);
        }

        /// <summary>
        /// Creates Debt
        /// </summary>
        /// <param name="debt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DebtDto> InsertDebt(DebtDto debt)
        {
            return await debtOperation.Value.InsertDebt(debt, userId);
        }

        /// <summary>
        /// Updates Debt
        /// </summary>
        /// <param name="debtId"></param>
        /// <param name="debt"></param>
        /// <returns></returns>
        [HttpPut("{debtId}")]
        public async Task UpdateDebt(int debtId, DebtDto debt)
        {
            debt.Id = debtId;
            debt.UserId = userId;

            await debtOperation.Value.UpdateDebt(debt, userId);
        }

        /// <summary>
        /// Closes Debt
        /// </summary>
        /// <param name="debtId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [AccountAccess]
        [HttpPatch("{debtId}")]
        public async Task CloseDebt(int debtId, int accountId)
        {
            await debtOperation.Value.CloseDebt(debtId, accountId, userId);
        }
    }
}
