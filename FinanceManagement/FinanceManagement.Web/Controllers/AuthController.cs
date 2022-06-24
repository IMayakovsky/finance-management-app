using FinanceManagement.Infrastructure.Attributes;
using FinanceManagement.Infrastructure.Dto.Auth;
using FinanceManagement.Infrastructure.Operations.Singletons;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Web.Controllers
{
    /// <summary>
    /// Controller for Authentication Operations
    /// </summary>
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly Lazy<IUserOperation> userOperation;
        private readonly Lazy<ITokenOperation> tokenOperation;
        private readonly Lazy<IAuthenticationOperation> authenticationOperation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userOperation"></param>
        /// <param name="authenticationManager"></param>
        /// <param name="tokenOperation"></param>
        public AuthController(Lazy<IUserOperation> userOperation, Lazy<IAuthenticationOperation> authenticationManager, Lazy<ITokenOperation> tokenOperation)
        {
            this.userOperation = userOperation;
            this.authenticationOperation = authenticationManager;
            this.tokenOperation = tokenOperation;
        }

        /// <summary>
        /// Validates user's email and password
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Access token if validation was successful, otherwise forbid result</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await userOperation.Value.GetUserByEmail(request.Email);

            if (user == null || request.Password == null)
            {
                return Unauthorized();
            }

            bool isValid = authenticationOperation.Value.ValidatePassword(request.Password, user.Password);

            if (!isValid)
            {
                return Forbid();
            }

            TokenResult res = authenticationOperation.Value.GenerateJwtTokens(user, request.RememberMe);

            await tokenOperation.Value.AddToken(res.RefreshToken);

            return Ok(new { accessToken = res.AccessToken });
        }

        /// <summary>
        /// Simple User Registration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task Register(RegisterRequest request)
        {
            await authenticationOperation.Value.Register(request);
        }

        /// <summary>
        /// Remove user's auth tokens from cache and database
        /// </summary>
        /// <returns></returns>
        [BaseAuthorize]
        [HttpGet]
        [Route("logout")]
        public async Task Logout()
        {
            string accessTokenId = HttpContext.User.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            string userId = HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.UserId).Value;

            await tokenOperation.Value.RemoveToken(int.Parse(userId), accessTokenId);
        }
    }
}
