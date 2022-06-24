using FinanceManagement.Core.Exceptions;
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
    /// Controller for Transaction operations
    /// </summary>
    [BaseAuthorize]
    [AccountAccess]
    [Route("api/accounts/{accountId}/transactions")]
    public class TransactionController : BaseController
    {
        private readonly Lazy<ITransactionOperation> transactionOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transactionOperation"></param>
        public TransactionController(Lazy<ITransactionOperation> transactionOperation)
        {
            this.transactionOperation = transactionOperation;
        }

        /// <summary>
        /// Returns Account's Transactions
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TransactionPageDto> GetAccountTransactions(int accountId, int currentPage, int pageSize)
        {
            return await transactionOperation.Value.GetTransactionsAndProfile(accountId, currentPage, pageSize);
        }

        /// <summary>
        /// Creates and returns transaction
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TransactionDto> CreateTransaction(int accountId, TransactionDto transaction)
        {
            transaction.AccountId = accountId;

            return await transactionOperation.Value.CreateTransaction(transaction, userId);
        }

        /// <summary>
        /// Deletes transaction
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpDelete("{transactionId}")]
        public async Task DeleteTransaction(int accountId, int transactionId)
        {
            await transactionOperation.Value.DeleteTransaction(transactionId, userId);
        }

        /// <summary>
        /// Updates transaction
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPut("{transactionId}")]
        public async Task UpdateTransaction(int accountId, int transactionId, TransactionDto transaction)
        {
            transaction.Id = transactionId;

            await transactionOperation.Value.UpdateTransaction(transaction, userId);
        }
    }
}
