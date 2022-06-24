using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Helpers;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Operations.Transients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Helpers
{
    public interface ISecurityHelper : IHelper
    {
        Task<bool> HasUserGroupRights(int userId, int groupId, IEnumerable<GroupRoleEnum> requiredRoles);
        Task<AccountDto> CheckUserAccountPermissions(int userId, int accountId);
    }

    public class SecurityHelper : ISecurityHelper
    {
        private readonly Lazy<IGroupOperation> groupOperations;
        private readonly Lazy<IAccountOperation> accountOperation;

        public SecurityHelper(Lazy<IGroupOperation> groupOperations, Lazy<IAccountOperation> accountOperation)
        {
            this.groupOperations = groupOperations;
            this.accountOperation = accountOperation;
        }

        public async Task<bool> HasUserGroupRights(int userId, int groupId, IEnumerable<GroupRoleEnum> requiredRoles)
        {
            var userRoles = await groupOperations.Value.GetUserGroupRolesCached(userId, groupId);

            var requiredRolesInt = requiredRoles.Cast<int>().ToList();

            return userRoles.Any(r => r.DateFrom <= DateTime.Now && r.DateTo >= DateTime.Now && requiredRolesInt.Contains((int)r.Role));
        }

        public async Task<AccountDto> CheckUserAccountPermissions(int userId, int accountId)
        {
            var account = (await accountOperation.Value.GetUserAccountsCached(userId, true)).FirstOrDefault((a) => a.Id == accountId);

            return account ?? throw new ForbiddenException($"User with Id {userId} doesn't have Account with Id {accountId}");
        }
    }
}
