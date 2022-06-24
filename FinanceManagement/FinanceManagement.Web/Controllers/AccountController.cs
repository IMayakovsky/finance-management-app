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
    /// Controller for Accounts Operations
    /// </summary>
    [BaseAuthorize]
    [Route("api/accounts")]
    public class AccountController : BaseController
    {
        private readonly Lazy<IAccountOperation> accountOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountOperation"></param>
        public AccountController(Lazy<IAccountOperation> accountOperation)
        {
            this.accountOperation = accountOperation;
        }

        /// <summary>
        /// Returns User's accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AccountDto>> GetUserAccounts()
        {
            return await accountOperation.Value.GetUserAccountsCached(userId);
        }

        /// <summary>
        /// Returns user's account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("{accountId}")]
        public async Task<AccountDto> GetUserAccount(int accountId)
        {
            return await accountOperation.Value.GetAccountByIdAndUserIdCached(accountId, userId);
        }

        /// <summary>
        /// Deletes Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpDelete("{accountId}")]
        public async Task DeleteAccount(int accountId)
        {
            await accountOperation.Value.DeleteAccount(accountId, userId);
        }

        /// <summary>
        /// Creates Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AccountDto> CreateAccount(AccountDto account)
        {
            return await accountOperation.Value.InsertAccount(account, userId);
        }

        /// <summary>
        /// Updates Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPut("{accountId}")]
        public async Task UpdateAccount(int accountId, AccountDto account)
        {
            account.Id = accountId;

            await accountOperation.Value.UpdateAccount(account, userId);
        }
    }
}
