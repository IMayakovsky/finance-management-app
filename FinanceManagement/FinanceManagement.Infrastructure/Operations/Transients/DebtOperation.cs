using Dawn;
using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto;
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
    public interface IDebtOperation : ITransientOperation
    {
        Task<List<DebtDto>> GetUserDebtsCached(int userId, bool ignoreCachedValues = false);
        Task<List<DebtDto>> GetExpiredDebts(int remainingDays);
        Task<DebtDto> InsertDebt(DebtDto Debt, int userId);
        Task UpdateDebt(DebtDto Debt, int userId);
        Task DeleteDebt(int debtId, int userId);
        Task CloseDebt(int debtId, int accountId, int userId);
    }

    public class DebtOperation : BaseInfrastructureOperation, IDebtOperation
    {
        private readonly IDataAccess dataAccess;
        private readonly ICacheInvalidationOperation cacheInvalidationOperation;
        private readonly Lazy<IAccountOperation> accountOperation;

        public DebtOperation(IDataAccess dataAccess, ICacheInvalidationOperation cacheInvalidationOperation, Lazy<IAccountOperation> accountOperation)
        {
            this.dataAccess = dataAccess;
            this.cacheInvalidationOperation = cacheInvalidationOperation;
            this.accountOperation = accountOperation;
        }

        public async Task<List<DebtDto>> GetUserDebtsCached(int userId, bool ignoreCachedValues = false)
        {
            return await this.ExecuteMemoryCachedAsync(async () => await GetDebtsByUserId(userId), 
                Cache.GetKey(typeof(DebtDto), typeof(UserDto), userId), 
                ignoreCachedValues, 
                new CacheDependency(CacheDependencyType.UserDebt, userId));
        }

        public async Task<List<DebtDto>> GetExpiredDebts(int remainingDays)
        {
            var models =  await dataAccess.Repository<IDebtRepository>().GetExpiredDebts(DateTime.UtcNow + TimeSpan.FromDays(remainingDays));

            return models.Select(m => m.Adapt<DebtDto>()).ToList();
        }

        public async Task<DebtDto> InsertDebt(DebtDto debt, int userId)
        {
            Guard.Argument(debt, nameof(debt)).NotNull();

            var model = debt.Adapt<Debt>();
            model.UserId = userId;

            await dataAccess.Repository<IDebtRepository>().InsertAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserDebt, userId);

            return model.Adapt<DebtDto>();
        }

        public async Task UpdateDebt(DebtDto debt, int userId)
        {
            Guard.Argument(debt, nameof(debt)).NotNull();

            var model = await dataAccess.Repository<IDebtRepository>().GetByIdAndUserId(debt.Id, userId);

            if (model == null)
            {
                throw new ForbiddenException();
            }

            var newModel = debt.Adapt<Debt>();
            newModel.UserId = userId;
            newModel.Created = model.Created;

            await dataAccess.Repository<IDebtRepository>().UpdateAndSaveAsync(newModel);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserDebt, userId);
        }

        public async Task DeleteDebt(int debtId, int userId)
        {
            var model = await dataAccess.Repository<IDebtRepository>().GetByIdAndUserId(debtId, userId);

            if (model == null)
            {
                throw new ForbiddenException();
            }

            await dataAccess.Repository<IDebtRepository>().RemoveAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserDebt, userId);
        }

        private async Task<List<DebtDto>> GetDebtsByUserId(int userId)
        {
            return (await dataAccess.Repository<IDebtRepository>().GetByUserId(userId)).Select(u => u.Adapt<DebtDto>()).ToList();
        }

        public async Task CloseDebt(int debtId, int accountId, int userId)
        {
            var model = await dataAccess.Repository<IDebtRepository>().GetByIdAndUserId(debtId, userId);
            var account = await accountOperation.Value.GetAccountByIdAndUserIdCached(accountId, userId) ?? throw new NotFoundException();

            if (model == null)
            {
                throw new NotFoundException();
            }
            if (account.Currency.ToString() != model.Currency)
            {
                throw new ForbiddenException("Debt and Account currencies are different");
            }

            await accountOperation.Value.IncreaseAccountAmmount(accountId, -model.Amount, userId);

            model.IsClosed = true;

            await dataAccess.Repository<IDebtRepository>().UpdateAndSaveAsync(model);
            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserDebt, userId);
        }
    }
}
