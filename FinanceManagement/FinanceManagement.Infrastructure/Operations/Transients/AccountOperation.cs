using Dawn;
using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Base;
using FinanceManagement.Infrastructure.Operations.Singletons;
using FinanceManagement.Infrastructure.Repositories;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Operations.Transients
{
    public interface IAccountOperation : ITransientOperation
    {
        Task<List<AccountDto>> GetUserAccountsCached(int userId, bool includeDeleted = false, bool ignoreCachedValue = false);
        Task<AccountDto> GetAccountByIdAndUserIdCached(int accountId, int userId);
        Task<AccountDto> InsertAccount(AccountDto account, int userId);
        Task UpdateAccount(AccountDto account, int userId);
        Task DeleteAccount(int accountId, int userId);
        Task<int> CreateAccount(string name, CurrencyEnum currency, int? userId = null);
        Task IncreaseAccountAmmount(int accountId, double amount, int? userId = null, int? groupId = null);
    }

    public class AccountOperation : BaseInfrastructureOperation, IAccountOperation
    {
        private readonly IDataAccess dataAccess;
        private readonly ICacheInvalidationOperation cacheInvalidationOperation;

        public AccountOperation(IDataAccess dataAccess, ICacheInvalidationOperation cacheInvalidationOperation)
        {
            this.dataAccess = dataAccess;
            this.cacheInvalidationOperation = cacheInvalidationOperation;
        }

        public async Task<List<AccountDto>> GetUserAccountsCached(int userId, bool includeDeleted = false, bool ignoreCachedValue = false)
        {
            var accounts = await this.ExecuteMemoryCachedAsync(async () => await GetAccountsByUserId(userId), 
                Cache.GetKey(typeof(AccountDto), typeof(UserDto), userId),
                ignoreCachedValue,
                new CacheDependency(CacheDependencyType.UserAccount, userId));

            return includeDeleted ? accounts : accounts.Where(a => !a.IsDeleted).ToList();
        }

        public async Task<AccountDto> GetAccountByIdAndUserIdCached(int accountId, int userId)
        {
            var userAccounts = await GetUserAccountsCached(userId);

            return userAccounts.FirstOrDefault((a) => a.Id == accountId);
        }

        public async Task<AccountDto> InsertAccount(AccountDto account, int userId)
        {
            Guard.Argument(account, nameof(Account)).NotNull();

            var model = account.Adapt<Account>();

            model.Code = $"{userId}-{account.Currency}-{account.Name}";
            model.UserId = userId;

            await dataAccess.Repository<IAccountRepository>().InsertAndSaveAsync(model);
            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserAccount, userId);

            return model.Adapt<AccountDto>();
        }

        public async Task<int> CreateAccount(string name, CurrencyEnum currency, int? userId = null)
        {
            var account = new Account
            {
                Name = name,
                Currency = currency.ToString(),
                Code = $"{userId}-{currency}-{name}",
                UserId = userId
            };

            await dataAccess.Repository<IAccountRepository>().InsertAndSaveAsync(account);

            if (userId.HasValue)
            {
                cacheInvalidationOperation.Invalidate(CacheDependencyType.UserAccount, userId.Value);
            }

            return account.Id;
        }

        public async Task UpdateAccount(AccountDto account, int userId)
        {
            Guard.Argument(account, nameof(Account)).NotNull();

            var model = account.Adapt<Account>();

            model.Code = $"{userId}-{account.Currency}-{account.Name}";
            model.UserId = userId;

            await dataAccess.Repository<IAccountRepository>().UpdateAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserAccount, userId);
        }

        public async Task DeleteAccount(int accountId, int userId)
        {
            var model = await dataAccess.Repository<IAccountRepository>().GetByIdAndUserId(accountId, userId);

            if (model == null)
            {
                throw new ForbiddenException();
            }

            model.IsDeleted = true;

            await dataAccess.Repository<IAccountRepository>().UpdateAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserAccount, userId);
        }

        public async Task IncreaseAccountAmmount(int accountId, double ammount, int? userId = null, int? groupId = null)
        {
            var account = await dataAccess.Repository<IAccountRepository>().GetById<Account, int>(accountId);

            account.Amount += ammount;

            await dataAccess.Repository<IAccountRepository>().UpdateAndSaveAsync(account);

            if (userId.HasValue)
            {
                cacheInvalidationOperation.Invalidate(CacheDependencyType.UserAccount, userId.Value);
            }

            if (groupId.HasValue)
            {
                cacheInvalidationOperation.Invalidate(CacheDependencyType.Group, groupId.Value);
            }
        }

        private async Task<List<AccountDto>> GetAccountsByUserId(int userId)
        {
            return (await dataAccess.Repository<IAccountRepository>().GetByUserId(userId)).Select(u => u.Adapt<AccountDto>()).ToList();
        }
    }
}
