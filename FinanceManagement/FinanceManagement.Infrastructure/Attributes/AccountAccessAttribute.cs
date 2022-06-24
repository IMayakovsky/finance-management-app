using FinanceManagement.Infrastructure.Dto.Auth;
using FinanceManagement.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Attributes
{
    public class AccountAccessAttribute : ActionFilterAttribute
    {
        protected const string headerKey = "accountId";

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.ContainsKey(headerKey))
            {
                context.Result = new ForbidResult($"AccountId not found in header {headerKey}");
                return;
            }

            int? accountId = null;
            int? userId = null;

            try
            {
                userId = int.Parse(context.HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.UserId).Value);
                accountId = int.Parse(context.ActionArguments[headerKey].ToString());

                var securityHelper = context.HttpContext.RequestServices.GetService<ISecurityHelper>();
             
                _ = await securityHelper.CheckUserAccountPermissions(userId.Value, accountId.Value);

                await next.Invoke();
            }
            catch
            {
                context.Result = new ForbidResult($"Account with Id {accountId} does not exist or user with Id {userId} has no access to it");
            }
        }
    }
}
