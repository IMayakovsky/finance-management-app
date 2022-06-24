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
    public interface IGoalOperation : ITransientOperation
    {
        Task<List<GoalDto>> GetUserGoalsCached(int userId, bool ignoreCachedValue = false);
        Task<List<GoalDto>> GetExpiredGoals(int remainingDays);
        Task<GoalDto> InsertGoal(GoalDto goal, int userId);
        Task UpdateGoal(GoalDto goal, int userId);
        Task DeleteGoal(int goalId, int userId);
        Task CloseGoal(int goalId, int userId);
        Task ChangeGoalAmount(int goalId, int accountId, double amount, int userId);
    }

    public class GoalOperation : BaseInfrastructureOperation, IGoalOperation
    {
        private readonly IDataAccess dataAccess;
        private readonly ICacheInvalidationOperation cacheInvalidationOperation;
        private readonly Lazy<IAccountOperation> accountOperation;

        public GoalOperation(IDataAccess dataAccess, ICacheInvalidationOperation cacheInvalidationOperation, Lazy<IAccountOperation> accountOperation)
        {
            this.dataAccess = dataAccess;
            this.cacheInvalidationOperation = cacheInvalidationOperation;
            this.accountOperation = accountOperation;
        }

        public async Task<List<GoalDto>> GetUserGoalsCached(int userId, bool ignoreCachedValue = false)
        {
            return await this.ExecuteMemoryCachedAsync(async () => await GetGoalsByUserId(userId), 
                Cache.GetKey(typeof(GoalDto), typeof(UserDto), userId),
                ignoreCachedValue,
                new CacheDependency(CacheDependencyType.UserGoal, userId));
        }

        public async Task<List<GoalDto>> GetExpiredGoals(int remainingDays)
        {
            var models =  await dataAccess.Repository<IGoalRepository>().GetExpiredGoals(DateTime.UtcNow + TimeSpan.FromDays(remainingDays));

            return models.Select(m => m.Adapt<GoalDto>()).ToList();
        }

        public async Task<GoalDto> InsertGoal(GoalDto goal, int userId)
        {
            Guard.Argument(goal, nameof(goal)).NotNull();

            var model = goal.Adapt<Goal>();
            model.UserId = userId;

            await dataAccess.Repository<IGoalRepository>().InsertAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserGoal, userId);

            return model.Adapt<GoalDto>();
        }

        public async Task UpdateGoal(GoalDto goal, int userId)
        {
            Guard.Argument(goal, nameof(goal)).NotNull();

            var model = await dataAccess.Repository<IGoalRepository>().GetByIdAndUserId(goal.Id, userId);

            if (model == null)
            {
                throw new ForbiddenException();
            }

            var newModel = goal.Adapt<Goal>();

            newModel.UserId = userId;
            newModel.Created = model.Created;

            await dataAccess.Repository<IGoalRepository>().UpdateAndSaveAsync(newModel);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserGoal, userId);
        }

        public async Task DeleteGoal(int goalId, int userId)
        {
            var model = await dataAccess.Repository<IGoalRepository>().GetByIdAndUserId(goalId, userId);

            if (model == null)
            {
                throw new ForbiddenException();
            }

            await dataAccess.Repository<IGoalRepository>().RemoveAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserGoal, userId);
        }

        public async Task ChangeGoalAmount(int goalId, int accountId, double amount, int userId)
        {
            var model = await dataAccess.Repository<IGoalRepository>().GetByIdAndUserId(goalId, userId) ?? throw new NotFoundException();
            var account = await accountOperation.Value.GetAccountByIdAndUserIdCached(accountId, userId) ?? throw new NotFoundException();

            if (amount > model.FullAmount)
            {
                throw new ForbiddenException("You cannot set the CurrentAmount of Goal greater than its FullAmount");
            }
            if (account.Currency.ToString() != model.Currency)
            {
                throw new ForbiddenException("Goal and Account currencies are different");
            }

            double diffrent = model.CurrentAmount - amount;

            await accountOperation.Value.IncreaseAccountAmmount(accountId, diffrent, userId);

            model.CurrentAmount = amount;

            await dataAccess.Repository<IGoalRepository>().UpdateAndSaveAsync(model);
            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserGoal, userId);
        }

        public async Task CloseGoal(int goalId, int userId)
        {
            var model = await dataAccess.Repository<IGoalRepository>().GetByIdAndUserId(goalId, userId) ?? throw new NotFoundException();

            if (model.FullAmount != model.CurrentAmount && model.CurrentAmount != 0)
            {
                throw new ForbiddenException("You cannot close the goal if CurrentAmout != FullAmount or CurrentAmout != 0");
            }

            model.IsActive = false;

            await dataAccess.Repository<IGoalRepository>().UpdateAndSaveAsync(model);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserGoal, userId);
        }

        private async Task<List<GoalDto>> GetGoalsByUserId(int userId)
        {
            return (await dataAccess.Repository<IGoalRepository>().GetByUserId(userId)).Select(u => u.Adapt<GoalDto>()).ToList();
        }
    }
}
