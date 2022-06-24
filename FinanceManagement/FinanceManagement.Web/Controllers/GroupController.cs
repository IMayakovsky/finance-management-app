using FinanceManagement.Infrastructure.Attributes;
using FinanceManagement.Infrastructure.Dto;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Dto.Groups;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManagement.Web.Controllers
{
    /// <summary>
    /// Controller for groups operations
    /// </summary>
    [BaseAuthorize]
    [Route("api/groups")]
    public class GroupController : BaseController
    {
        private readonly Lazy<IGroupOperation> groupOperation;
        private readonly Lazy<ITransactionOperation> transactionOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="groupOperation"></param>
        public GroupController(Lazy<IGroupOperation> groupOperation, Lazy<ITransactionOperation> transactionOperation)
        {
            this.groupOperation = groupOperation;
            this.transactionOperation = transactionOperation;
        }

        /// <summary>
        /// Returns User's Groups
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<GroupDto>> GetUserGroups()
        {
            return await groupOperation.Value.GetUserGroupsCached(userId);
        }

        /// <summary>
        /// Returns Group
        /// </summary>
        /// <returns></returns>
        [HttpGet("{groupId}")]
        public async Task<GroupDto> GetUserGroup(int groupId)
        {
            return await groupOperation.Value.GetUserGroupCached(groupId, userId);
        }

        /// <summary>
        /// Creates Group with new Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GroupDto> CreateGroup(GroupCreateRequest request)
        {
            return await groupOperation.Value.CreateGroup(request, userId);
        }

        /// <summary>
        /// Updates Group
        /// </summary>
        /// <returns></returns>
        [GroupRoles(GroupRoleEnum.Admin)]
        [HttpPut("{groupId}")]
        public async Task UpdateGroup(int groupId, GroupDto request)
        {
            request.Id = groupId;

            await groupOperation.Value.UpdateGroup(request);
        }

        /// <summary>
        /// Deletes Group
        /// </summary>
        /// <returns></returns>
        [GroupRoles(GroupRoleEnum.Admin)]
        [HttpDelete("{groupId}")]
        public async Task DeleteGroup(int groupId)
        {
            await groupOperation.Value.DeleteGroup(groupId);
        }

        /// <summary>
        /// Returns Group's Transactions
        /// </summary>
        /// <returns></returns>
        [GroupRoles(GroupRoleEnum.Admin, GroupRoleEnum.Editor, GroupRoleEnum.Reader)]
        [HttpGet("{groupId}/transactions")]
        public async Task<TransactionPageDto> GetGroupTransactions(int groupId, int currentPage, int pageSize)
        {
            return await groupOperation.Value.GetGroupTransaction(groupId, currentPage, pageSize);
        }

        /// <summary>
        /// Creates Group's Transaction
        /// </summary>
        /// <returns></returns>
        [GroupRoles(GroupRoleEnum.Admin, GroupRoleEnum.Editor)]
        [HttpPost("{groupId}/transactions")]
        public async Task CreateGroupTransaction(int groupId, TransactionDto transactionDto)
        {
            await groupOperation.Value.AddTransaction(groupId, transactionDto);
        }
        
        /// <summary>
        /// Updates Group's Transaction
        /// </summary>
        /// <returns></returns>
        [GroupRoles(GroupRoleEnum.Admin, GroupRoleEnum.Editor)]
        [HttpPut("{groupId}/transactions/{transactionId}")]
        public async Task UpdateGroupTransaction(int groupId, int transactionId, TransactionDto transactionDto)
        {
            transactionDto.Id = transactionId;

            await transactionOperation.Value.UpdateTransaction(transactionDto, null, groupId);
        }

        /// <summary>
        /// Deletes Group's Transaction
        /// </summary>
        /// <returns></returns>
        [GroupRoles(GroupRoleEnum.Admin, GroupRoleEnum.Editor)]
        [HttpDelete("{groupId}/transactions/{transactionId}")]
        public async Task UpdateGroupTransaction(int groupId, int transactionId)
        {
            await transactionOperation.Value.DeleteTransaction(transactionId, null, groupId);
        }
    }
}
