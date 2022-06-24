using FinanceManagement.Core.Exceptions;
using FinanceManagement.Infrastructure.Dto.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FinanceManagement.Web.Controllers
{
    /// <summary>
    /// Base Controller
    /// </summary>
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Current User ID
        /// </summary>
        protected int userId => int.Parse(HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.UserId).Value);

        /// <summary>
        /// Returns true if current user is Admin
        /// </summary>
        protected bool isAdmin => bool.Parse(HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.IsAdmin).Value);
    }
}
