using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.AspNetCore.Mvc;
using FinanceManagement.Infrastructure.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using FinanceManagement.Infrastructure.Dto.Auth;

namespace FinanceManagement.Infrastructure.Attributes
{
    public class BaseAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public BaseAuthorizeAttribute() { }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            TokenResult result = await VerifyToken(context.HttpContext);

            if (!result.Success)
            {
                context.Result = new UnauthorizedObjectResult(result.Error);
            }
        }

        private async Task<TokenResult> VerifyToken(HttpContext context)
        {
            TokenResult result = new TokenResult { Success = false };

            string accessTokenId, userId;

            try
            {
                accessTokenId = context.User.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                userId = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.UserId).Value;
            }
            catch (Exception)
            {
                result.Error = "Invalid access token format";
                return result;
            }

            var tokenOperation = context.RequestServices.GetService<ITokenOperation>();

            RefreshTokenDto storedRefreshToken = await tokenOperation.GetTokenCached(int.Parse(userId), accessTokenId);

            if (accessTokenId == null || storedRefreshToken == null)
            {
                result.Error = "Refresh token does not exist";
                return result;
            }

            if (storedRefreshToken.AccessTokenId != accessTokenId)
            {
                result.Error = "The token does not mateched the saved refresh token";
                return result;
            }

            result.Success = true;
            return result;
        }
    }
}
