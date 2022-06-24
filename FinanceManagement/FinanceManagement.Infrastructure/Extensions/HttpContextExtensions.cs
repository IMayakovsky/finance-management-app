using FinanceManagement.Infrastructure.Dto.Auth;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FinanceManagement.Infrastructure.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            return context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.UserId)?.Value;
        }
    }
}
