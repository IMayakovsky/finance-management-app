using FinanceManagement.Infrastructure.Dto.Auth;
using FinanceManagement.Infrastructure.Operations.Singletons;
using FinanceManagement.Infrastructure.Operations.Transients;
using FinanceManagement.MessageService.Factories;
using FinanceManagement.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FinanceManagement.Controllers
{
    /// <summary>
    /// Controller for password restoring operations
    /// </summary>
    [ApiController]
    [Route("api/restore/password")]
    public class PasswordRestoreController : BaseController
    {
        private readonly IAuthenticationOperation authenticationOperation;
        private readonly IUserOperation userOperation;
        private readonly ITokenOperation tokenOperation;
        private readonly Lazy<IMessageOperation> messageOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authenticationOperation"></param>
        /// <param name="userOperation"></param>
        /// <param name="tokenOperation"></param>
        /// <param name="messageOperation"></param>
        public PasswordRestoreController(IAuthenticationOperation authenticationOperation, 
            IUserOperation userOperation,
            ITokenOperation tokenOperation, 
            Lazy<IMessageOperation> messageOperation)
        {
            this.authenticationOperation = authenticationOperation;
            this.userOperation = userOperation;
            this.tokenOperation = tokenOperation;
            this.messageOperation = messageOperation;
        }

        /// <summary>
        /// Validates hash id and restore user's password
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Ok if password was restored, Forbid otherwise</returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> RestorePassword(RestorePasswordRequest request)
        {
            var response = await authenticationOperation.RestorePassword(request);

            if (!response.Success)
                return Forbid();

            await tokenOperation.RemoveTokensByUserId(response.UserId);

            return Ok();
        }


        /// <summary>
        /// Creates password restore request, sends email to user with reset link
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="link">redirect link</param>
        /// <returns>Ok result if user was found, BadRequest Result otherwise</returns>
        [Route("request")]
        [HttpPost]
        public async Task<IActionResult> RequestPasswordReset(string email, string link)
        {
            string userRestoreId = await authenticationOperation.StoreUserResetId(email);

            if (userRestoreId == null)
                return BadRequest();

            var userInfo = await userOperation.GetUserByEmail(email);

            var message = MessagesFactory.EmailResetPasswordMessage(email, userInfo.Name, $"{link}/{userRestoreId}");

            await messageOperation.Value.CreateMessage(message);

            return Ok();
        }

        /// <summary>
        /// Validates user restore id with value in database.
        /// </summary>
        /// <param name="userRestoreId"></param>
        /// <returns>Ok Result if validation was successful, otherwise Forbid Result</returns>
        [Route("{userRestoreId}/valid")]
        [HttpGet]
        public async Task<IActionResult> CheckUserRestoreId(string userRestoreId)
        {
            bool isRequestValid = await authenticationOperation.CompareWithStoredUserResetId(userRestoreId);

            return isRequestValid ? Ok() : Forbid();
        }
    }
}
