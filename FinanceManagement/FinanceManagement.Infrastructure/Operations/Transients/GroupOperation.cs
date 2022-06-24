using Dawn;
using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Operations;
using FinanceManagement.Infrastructure.Database;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Dto.Groups;
using FinanceManagement.Infrastructure.Mappers;
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
    public interface IGroupOperation : ITransientOperation
    {
        Task PreloadGroups(List<int> ids);
        Task<List<int>> GetGroupIds(int count, int skip);
        Task<List<UserGroupRolesDto>> GetUserGroupRolesCached(int userId, int groupId);
        Task<UserGroupsRolesDto> GetUserGroupsRolesCached(int userId, bool onlyActive, bool ignoreCachedValue = false);
        Task<List<GroupDto>> GetUserGroupsCached(int userId, bool ignoreCachedValue = false);
        Task<GroupDto> GetUserGroupCached(int groupId, int userId);
        Task<List<GroupDto>> GetGroupsCached(List<int> groupIds, bool ignoreCachedValue = false);
        Task<GroupDto> CreateGroup(GroupCreateRequest request, int userId);
        Task UpdateGroup(GroupDto group);
        Task DeleteGroup(int groupId);
        Task<TransactionPageDto> GetGroupTransaction(int groupId, int currentPage, int pageSize);
        Task<TransactionDto> AddTransaction(int groupId, TransactionDto transaction);
    }

    public class GroupOperation : BaseInfrastructureOperation, IGroupOperation
    {
        private readonly IDataAccess dataAccess;
        private readonly Lazy<IAccountOperation> accountOperation;
        private readonly Lazy<ICacheInvalidationOperation> cacheInvalidationOperation;
        private readonly Lazy<ITransactionOperation> transactionOperation;

        public GroupOperation(IDataAccess dataAccess,
            Lazy<IAccountOperation> accountOperation,
            Lazy<ICacheInvalidationOperation> cacheInvalidationOperation, 
            Lazy<ITransactionOperation> transactionOperation)
        {
            this.dataAccess = dataAccess;
            this.accountOperation = accountOperation;
            this.cacheInvalidationOperation = cacheInvalidationOperation;
            this.transactionOperation = transactionOperation;
        }

        public async Task PreloadGroups(List<int> ids)
        {
            await GetGroupsCached(ids, true);
        }

        public async Task<List<int>> GetGroupIds(int count, int skip)
        {
            return await this.dataAccess.Repository<IGroupRepository>().GetGroupIdsPage(count, skip);
        }

        public async Task<List<UserGroupRolesDto>> GetUserGroupRolesCached(int userId, int groupId)
        {
            var roles = await GetUserGroupsRolesCached(userId, false);

            return roles.Roles.ContainsKey(groupId) ? roles.Roles[groupId] : new List<UserGroupRolesDto>();
        }

        public async Task<UserGroupsRolesDto> GetUserGroupsRolesCached(int userId, bool onlyActive, bool ignoreCachedValue = false)
        {
            var roles = await this.ExecuteMemoryCachedAsync(async () => await GetUserGroupsRoles(userId),
                Cache.GetKey(typeof(UserGroupsRolesDto), typeof(UserDto), userId),
                ignoreCachedValue,
                new CacheDependency(CacheDependencyType.UserGroupRole, userId));

            if (onlyActive)
            {
                roles.Roles = roles.Roles.Where(e => e.Value.Any(r => r.DateFrom <= DateTime.Now && r.DateTo >= DateTime.Now)).ToDictionary(r => r.Key, r => r.Value);
            }

            return roles;
        }

        public async Task<List<GroupDto>> GetUserGroupsCached(int userId, bool ignoreCachedValue = false)
        {
            var roles = await GetUserGroupsRolesCached(userId, true, ignoreCachedValue);

            return await GetGroupsCached(roles.GroupIds);
        }

        public async Task<GroupDto> GetUserGroupCached(int groupId, int userId)
        {
            var groups = await GetUserGroupsCached(userId);

            return groups.FirstOrDefault(g => g.Id == groupId);
        }

        public async Task<List<GroupDto>> GetGroupsCached(List<int> groupIds, bool ignoreCachedValue = false)
        {
            return await this.ExecuteMemoryCachedMultipleAsync(groupIds,
                id => Cache.GetKey(typeof(GroupDto), id),
                user => user.Id,
                GetGroups,
                group => new[] { new CacheDependency(CacheDependencyType.Group, group.Id) },
                ignoreCachedValue);
        }

        public async Task<GroupDto> CreateGroup(GroupCreateRequest request, int userId)
        {
            Guard.Argument(request, nameof(request)).NotNull();

            var groups = await GetUserGroupsCached(userId);

            if (groups.Any(g => g.Name == request.GroupName))
            {
                throw new BaseException("Group with the same name already exists");
            }

            var accountName = $"groupAccount-{request.GroupName}";
            var accountId = await accountOperation.Value.CreateAccount(accountName, request.Currency);

            var group = new Group
            {
                AccountId = accountId,
                Name = request.GroupName,
            };

            await dataAccess.Repository<IGroupRepository>().InsertAndSaveAsync(group);

            var role = new UserGroupRole
            {
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.MaxValue,
                GroupId = group.Id,
                GroupRoleId = (int)GroupRoleEnum.Admin,
                UserId = userId,
            };

            await dataAccess.Repository<IUserGroupRoleRepository>().InsertAndSaveAsync(role);

            cacheInvalidationOperation.Value.Invalidate(CacheDependencyType.UserGroupRole, userId);

            return group.Adapt<GroupDto>();
        }

        public async Task UpdateGroup(GroupDto request)
        {
            var model = await dataAccess.Repository<IGroupRepository>().GetById(request.Id) ?? throw new NotFoundException();

            model.Name = request.Name;
            dataAccess.Repository<IGroupRepository>().Update(model);

            var groupRoles = await dataAccess.Repository<IUserGroupRoleRepository>().GetByGroupId(request.Id);
            List<UserGroupRole> groupRolesToCreate = request.Roles.Select(r => new UserGroupRole { DateFrom = r.DateFrom, DateTo = r.DateTo, GroupId = model.Id, UserId = r.UserId, GroupRoleId = (int) r.Role }).ToList();

            dataAccess.Repository<IUserGroupRoleRepository>().RemoveRange(groupRoles);
            dataAccess.Repository<IUserGroupRoleRepository>().InsertRange(groupRolesToCreate);

            await dataAccess.DbContext.SaveChangesAsync();

            var userIdsForCacheInvalidation = groupRoles.Select(e => e.UserId).ToList();
            userIdsForCacheInvalidation.AddRange(request.Roles.Select(e => e.UserId));

            cacheInvalidationOperation.Value.Invalidate(CacheDependencyType.Group, request.Id);
            cacheInvalidationOperation.Value.Invalidate(CacheDependencyType.UserGroupRole, userIdsForCacheInvalidation.Distinct().ToArray());
        }

        public async Task DeleteGroup(int groupId)
        {
            var model = (await dataAccess.Repository<IGroupRepository>().GetByIdsWithRolesAndAcount(new List<int> { groupId })).FirstOrDefault() ?? throw new NotFoundException();

            model.Account.IsDeleted = true;

            await dataAccess.Repository<IAccountRepository>().UpdateAndSaveAsync(model.Account);

            cacheInvalidationOperation.Value.Invalidate(CacheDependencyType.Group, groupId);
            cacheInvalidationOperation.Value.Invalidate(CacheDependencyType.UserGroupRole, model.UserGroupRoles.Select(e => e.UserId).ToArray());
        }

        public async Task<TransactionPageDto> GetGroupTransaction(int groupId, int currentPage, int pageSize)
        {
            var accountId = await GetGroupAccount(groupId);
            return await transactionOperation.Value.GetTransactionsAndProfile(accountId, currentPage, pageSize);
        }

        public async Task<TransactionDto> AddTransaction(int groupId, TransactionDto transaction)
        {
            Guard.Argument(transaction, nameof(transaction)).NotNull();

            transaction.AccountId = await GetGroupAccount(groupId);

            return await transactionOperation.Value.CreateTransaction(transaction, null, groupId);
        }

        private async Task<UserGroupsRolesDto> GetUserGroupsRoles(int userId)
        {
            var result = new UserGroupsRolesDto
            {
                UserId = userId,
                Roles = new Dictionary<int, List<UserGroupRolesDto>>()
            };

            var roles = await dataAccess.Repository<IUserGroupRoleRepository>().GetByUserId(userId);

            foreach (var role in roles)
            {
                result.Roles.Add(role.Key, role.Select(x => x.Adapt<UserGroupRolesDto>()).ToList());
            }

            return result;
        }

        private async Task<int> GetGroupAccount(int groupId)
        {
            var group = (await GetGroupsCached(new List<int> { groupId })).FirstOrDefault() ?? throw new NotFoundException();

            return group.AccountId;
        }

        private async Task<List<GroupDto>> GetGroups(List<int> groupIds)
        {
            var groups = await dataAccess.Repository<IGroupRepository>().GetByIdsWithRolesAndAcount(groupIds);
            return groups.Select(CommonMapper.Map).ToList();
        }
    }
}
