using Dawn;
using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Common.Constants;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Hubs;
using FinanceManagement.Infrastructure.Models.Generated;
using FinanceManagement.Infrastructure.Operations.Base;
using FinanceManagement.Infrastructure.Operations.Singletons;
using FinanceManagement.Infrastructure.Repositories;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Operations.Transients
{
    public interface ITransactionOperation : ITransientOperation
    {
        Task<TransactionPageDto> GetTransactionsAndProfile(int accountId, int page, int pageSize);
        Task<TransactionPageDto> GetTransactions(int accountId, int page, int pageSize);
        Task<TransactionDto> GetTransactionByUserIdAndId(int userId, int transactionId);
        Task<List<TransactionDto>> GetLastAccountTransactionsCached(int accountId, bool ignoreCachedValue = false);
        Task<TransactionDto> CreateTransaction(TransactionDto transaction, int? userId = null, int? groupId = null);
        Task UpdateTransaction(TransactionDto transaction, int? userId = null, int? groupId = null);
        Task DeleteTransaction(int transactionId, int? userId = null, int? groupId = null);
        Task<List<CategoryDto>> GetUserCategoriesCached(int userId, bool ignoreCachedValue = false);
        Task<List<CategoryDto>> GetDefaultCategoriesCached(bool ignoreCachedValue = false);
        Task<int> InsertUserCategory(int userId, CategoryDto category);
        Task UpdateUserCategoryName(int userId, int categoryId, string name);
        Task DeleteUserCategory(int userId, int categoryId);
    }

    public class TransactionOperation : BaseInfrastructureOperation, ITransactionOperation
    {
        private readonly IDataAccess dataAccess;
        private readonly ICacheInvalidationOperation cacheInvalidationOperation;
        private readonly Lazy<IAccountOperation> accountOperation;
        private readonly IHubContext<NotificationHub, INotificationHub> notificationHubContext;
        private readonly Lazy<IGroupOperation> groupOperation;

        public TransactionOperation(IDataAccess dataAccess,
            ICacheInvalidationOperation cacheInvalidationOperation,
            Lazy<IAccountOperation> accountOperation,
            IHubContext<NotificationHub, INotificationHub> notificationHubContext, 
            Lazy<IGroupOperation> groupOperation)
        {
            this.dataAccess = dataAccess;
            this.cacheInvalidationOperation = cacheInvalidationOperation;
            this.accountOperation = accountOperation;
            this.notificationHubContext = notificationHubContext;
            this.groupOperation = groupOperation;
        }

        public async Task<TransactionPageDto> GetTransactionsAndProfile(int accountId, int page, int pageSize)
        {
            return await ExecuteAndProfile(() => GetTransactions(accountId, page, pageSize));
        }

        public async Task<TransactionPageDto> GetTransactions(int accountId, int page, int pageSize)
        {
            var res = await dataAccess.Repository<ITransactionRepository>().GetByAccountId(accountId, page, pageSize);

            return new TransactionPageDto
            {
                TotalRowCount = res.Count,
                Transactions = res.Entities.Select(u => u.Adapt<TransactionDto>()).ToList()
            };
        }

        public async Task<TransactionDto> GetTransactionByUserIdAndId(int userId, int transactionId)
        {
            var res = await dataAccess.Repository<ITransactionRepository>().GetByIdAndUserId(transactionId, userId);

            return res.Adapt<TransactionDto>();
        }

        public async Task<List<TransactionDto>> GetLastAccountTransactionsCached(int accountId, bool ignoreCachedValue = false)
        {
            return await this.ExecuteMemoryCachedAsync(async () => await GetLastAccountTransactions(accountId),
                Cache.GetKey(typeof(List<TransactionDto>), typeof(AccountDto), accountId),
                ignoreCachedValue,
                new CacheDependency(CacheDependencyType.AccountLastTransaction, accountId));
        }

        public async Task<TransactionDto> CreateTransaction(TransactionDto transaction, int? userId = null, int? groupId = null)
        {
            Guard.Argument(transaction, nameof(transaction)).NotNull();

            var model = transaction.Adapt<Transaction>();

            await dataAccess.Repository<ITransactionRepository>().InsertAndSaveAsync(model);
            await accountOperation.Value.IncreaseAccountAmmount(model.AccountId, model.Amount, userId, groupId);
            cacheInvalidationOperation.Invalidate(CacheDependencyType.AccountLastTransaction, model.AccountId);

            if (groupId.HasValue)
            {
                var group = (await groupOperation.Value.GetGroupsCached(new List<int> { groupId.Value })).FirstOrDefault();
                var groupUsers = group.Roles.Select(r => r.UserId).Distinct();
                var notifications = new List<Notification>();

                foreach (var id in groupUsers)
                {
                    var notification = new Notification
                    {
                        IsRead = false,
                        Name = NotificationNameEnum.GroupTransaction.ToString(),
                        NotificationTypeId = (int)NotificationTypeEnum.Information,
                        Parameters = JsonConvert.SerializeObject(transaction),
                        UserId = id,
                    };
                    notifications.Add(notification);
                }

                await dataAccess.Repository<INotificationRepository>().InsertRangeAndSaveAsync(notifications);
                await notificationHubContext.Clients.Groups(groupUsers.Select(id => id.ToString())).SendNotification(notifications.First().Adapt<NotificationDto>());
            }

            return model.Adapt<TransactionDto>();
        }

        public async Task UpdateTransaction(TransactionDto transaction, int? userId = null, int? groupId = null)
        {
            Guard.Argument(transaction, nameof(transaction)).NotNull();

            var model = await dataAccess.Repository<ITransactionRepository>().GetById<Transaction, int>(transaction.Id);

            var changedAmount = transaction.Amount - model.Amount;

            model.Amount = transaction.Amount;
            model.Name = transaction.Name;
            model.Date = transaction.Date;

            if (transaction.CategoryId.HasValue)
            {
                var category = await dataAccess.Repository<ICategoryRepository>().GetById<Category, int>(transaction.CategoryId.Value);

                if (category != null && category.UserId == userId)
                {
                    model.CategoryId = transaction.CategoryId;
                }
            }

            await dataAccess.Repository<ITransactionRepository>().UpdateAndSaveAsync(model);
            await accountOperation.Value.IncreaseAccountAmmount(model.AccountId, changedAmount, userId, groupId);
            cacheInvalidationOperation.Invalidate(CacheDependencyType.AccountLastTransaction, model.AccountId);
        }

        public async Task DeleteTransaction(int transactionId, int? userId = null, int? groupId = null)
        {
            var model = await dataAccess.Repository<ITransactionRepository>().GetById<Transaction, int>(transactionId);

            if (model == null)
            {
                throw new ForbiddenException();
            }

            await dataAccess.Repository<ITransactionRepository>().RemoveAndSaveAsync(model);
            await accountOperation.Value.IncreaseAccountAmmount(model.AccountId, -model.Amount, userId, groupId);
            cacheInvalidationOperation.Invalidate(CacheDependencyType.AccountLastTransaction, model.AccountId);
        }

        public async Task<List<CategoryDto>> GetUserCategoriesCached(int userId, bool ignoreCachedValue = false)
        {
            var userCategories = await this.ExecuteMemoryCachedAsync(async () => await GetUserCategories(userId),
                Cache.GetKey(typeof(CategoryDto), typeof(UserDto), userId),
                ignoreCachedValue,
                new CacheDependency(CacheDependencyType.UserCategory, userId));

            var result = new List<CategoryDto>();
            result.AddRange(userCategories);
            result.AddRange(await GetDefaultCategoriesCached());

            return result;
        }

        public async Task<List<CategoryDto>> GetDefaultCategoriesCached(bool ignoreCachedValue = false)
        {
            return await this.ExecuteMemoryCachedAsync(async () => await GetDefaultCategories(),
                Cache.GetKey(typeof(List<CategoryDto>)),
                ignoreCachedValue);
        }


        public async Task<int> InsertUserCategory(int userId, CategoryDto category)
        {
            Guard.Argument(userId, nameof(userId)).NotNegative().NotZero();

            var model = category.Adapt<Category>();

            model.UserId = userId;

            await dataAccess.Repository<ICategoryRepository>().InsertAndSaveAsync(model);
            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserCategory, userId);

            return model.Id;
        }

        public async Task UpdateUserCategoryName(int userId, int categoryId, string name)
        {
            Guard.Argument(userId, nameof(userId)).NotNegative().NotZero();

            var model = await dataAccess.Repository<ICategoryRepository>().GetByIdAndUserId(categoryId, userId);

            if (model == null)
            {
                throw new NotFoundException();
            }

            model.Name = name;

            await dataAccess.Repository<ICategoryRepository>().UpdateAndSaveAsync(model);
            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserCategory, userId);
        }

        public async Task DeleteUserCategory(int userId, int categoryId)
        {
            Guard.Argument(userId, nameof(userId)).NotNegative().NotZero();

            var model = await dataAccess.Repository<ICategoryRepository>().GetByIdAndUserId(categoryId, userId);

            if (model == null)
            {
                return;
            }

            await dataAccess.Repository<ICategoryRepository>().RemoveAndSaveAsync(model);
            cacheInvalidationOperation.Invalidate(CacheDependencyType.UserCategory, model.UserId.Value);
        }

        private async Task<List<CategoryDto>> GetDefaultCategories()
        {
            return (await dataAccess.Repository<ICategoryRepository>().GetDefaultCategories()).Select(m => m.Adapt<CategoryDto>()).ToList();
        }

        private async Task<List<CategoryDto>> GetUserCategories(int userId)
        {
            return (await dataAccess.Repository<ICategoryRepository>().GetByUserId(userId)).Select(m => m.Adapt<CategoryDto>()).ToList();
        }

        private async Task<List<TransactionDto>> GetLastAccountTransactions(int accountId)
        {
            return (await dataAccess.Repository<ITransactionRepository>().GetByAccountId(accountId, 1, DashboardConstants.TopDataCount)).Entities.Select(u => u.Adapt<TransactionDto>()).ToList();
        }
    }
}
