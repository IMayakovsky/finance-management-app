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
    /// Controller for User Operations
    /// </summary>
    [BaseAuthorize]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly Lazy<IUserOperation> userOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserController(Lazy<IUserOperation> userOperation)
        {
            this.userOperation = userOperation;
        }

        /// <summary>
        /// Returns UserData if access token in request's header is valid.
        /// </summary>
        /// <returns>User Data</returns>
        /// <exception cref="NotFoundException"></exception>
        [HttpGet("me")]
        public async Task<UserInfoDto> Me()
        {
            return await userOperation.Value.GetUserInfoByIdCached(userId);
        }

        /// <summary>
        /// Returns Users
        /// </summary>
        [HttpGet]
        public async Task<List<UserInfoDto>> GetUsers(string name, int currentPage, int pageSize)
        {
            return await userOperation.Value.GetUsersPage(name, currentPage, pageSize);
        }
    }
}
