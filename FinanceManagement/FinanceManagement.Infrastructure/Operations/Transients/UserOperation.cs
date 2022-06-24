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
    public interface IUserOperation : ITransientOperation
    {
        Task PreloadUsers(List<int> ids, bool onlyUserModelData);
        Task<List<int>> GetUserIds(int count, int skip);
        Task<User> GetUserByEmail(string email);
        Task<UserInfoDto> GetUserInfoByIdCached(int userId);
        Task<UserDto> GetUserByIdCached(int userId);
        Task ChangeUserName(int userId, string name);
        Task<List<UserInfoDto>> GetUsersPage(string name, int currentPage, int pageSize);
    }

    public class UserOperation : BaseInfrastructureOperation, IUserOperation
    {
        private readonly IDataAccess dataAccess;
        private readonly ICacheInvalidationOperation cacheInvalidationOperation;
        private readonly Lazy<ISubscriptionOperation> subscriptionOperation;
        private readonly Lazy<IAccountOperation> accountOperation;
        private readonly Lazy<IGroupOperation> groupOperations;
        private readonly Lazy<ITransactionOperation> transactionOperation;

        public UserOperation(IDataAccess dataAccess, ICacheInvalidationOperation cacheInvalidationOperation,
            Lazy<ISubscriptionOperation> subscriptionOperation, Lazy<IAccountOperation> accountOperation,
            Lazy<IGroupOperation> groupOperations, Lazy<ITransactionOperation> transactionOperation)
        {
            this.dataAccess = dataAccess;
            this.cacheInvalidationOperation = cacheInvalidationOperation;
            this.subscriptionOperation = subscriptionOperation;
            this.accountOperation = accountOperation;
            this.groupOperations = groupOperations;
            this.transactionOperation = transactionOperation;
        }

        public async Task PreloadUsers(List<int> ids, bool onlyUserModelData)
        {
            await GetUsersByIdsCached(ids, true);

            if (onlyUserModelData)
            {
                return;
            } 
                
            foreach (var id in ids)
            {
                await groupOperations.Value.GetUserGroupsCached(id, true);

                var accounts = await accountOperation.Value.GetUserAccountsCached(id, true, true);
                await subscriptionOperation.Value.GetAccountSubscriptionsCached(accounts.Select(a => a.Id).ToList(), true);

                foreach (var account in accounts)
                {
                    await transactionOperation.Value.GetLastAccountTransactionsCached(account.Id, true);
                }
            }
        }

        public async Task<List<int>> GetUserIds(int count, int skip)
        {
            return await this.dataAccess.Repository<IUserRepository>().GetUserIdsPage(count, skip);
        }

        public async Task<List<UserDto>> GetUsersByIdsCached(List<int> userIds, bool ignoreCachedValue = false)
        {
            Guard.Argument(userIds, nameof(userIds)).NotNull();

            return await this.ExecuteMemoryCachedMultipleAsync(userIds,
                id => Cache.GetKey(typeof(UserDto), id),
                user => user.Id,
                GetUsersByIds,
                profile => new[] { new CacheDependency(CacheDependencyType.User, profile.Id) },
                ignoreCachedValue);
        }

        public async Task<UserDto> GetUserByIdCached(int userId)
        {
            return await this.ExecuteMemoryCachedAsync(async () => await GetUser(userId), Cache.GetKey(typeof(UserDto), userId), new CacheDependency(CacheDependencyType.User, userId));
        }

        public async Task<UserInfoDto> GetUserInfoByIdCached(int userId)
        {
            var user = await GetUserByIdCached(userId) ?? throw new NotFoundException();

            return user.Adapt<UserInfoDto>();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await dataAccess.Repository<IUserRepository>().GetByEmail(email);
        }

        public async Task ChangeUserName(int userId, string name)
        {
            var user = await dataAccess.Repository<IUserRepository>().GetById(userId);

            if (user == null)
            {
                throw new BaseException($"User with id {userId} was not found");
            }

            user.Name = name;

            await this.dataAccess.Repository<IUserRepository>().UpdateAndSaveAsync(user);

            cacheInvalidationOperation.Invalidate(CacheDependencyType.User, userId);
        }

        public async Task<List<UserInfoDto>> GetUsersPage(string name, int currentPage, int pageSize)
        {
            var models = await dataAccess.Repository<IUserRepository>().GetUsers(name, currentPage, pageSize);

            return models.Select(m => m.Adapt<UserInfoDto>()).ToList();
        }

        private async Task<List<UserDto>> GetUsersByIds(List<int> userIds)
        {
            var models = await dataAccess.Repository<IUserRepository>().GetByIds(userIds);

            return models.Select(m => m.Adapt<UserDto>()).ToList();
        }

        private async Task<UserDto> GetUser(int userId)
        {
            return (await dataAccess.Repository<IUserRepository>().GetById(userId)).Adapt<UserDto>();
        }
    }
}
