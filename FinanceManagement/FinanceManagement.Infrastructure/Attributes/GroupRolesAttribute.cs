using FinanceManagement.Infrastructure.Dto.Auth;
using FinanceManagement.Infrastructure.Dto.Enums;
using FinanceManagement.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Attributes
{
    public class GroupRolesAttribute : ActionFilterAttribute
    {
        protected const string headerKey = "groupId";

        protected GroupRoleEnum[] roles;

        public GroupRolesAttribute(params GroupRoleEnum[] roles)
        {
            this.roles = roles;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.ContainsKey(headerKey))
            {
                context.Result = new UnauthorizedObjectResult($"GroupId not found in header {headerKey}");
                return;
            }

            try
            {
                int userId = int.Parse(context.HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.UserId).Value);
                int groupId = int.Parse(context.ActionArguments[headerKey].ToString());

                var securityHelper = context.HttpContext.RequestServices.GetService<ISecurityHelper>();

                var hasAccess = await securityHelper.HasUserGroupRights(userId, groupId, roles);

                if (hasAccess)
                {
                    await next.Invoke();
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
