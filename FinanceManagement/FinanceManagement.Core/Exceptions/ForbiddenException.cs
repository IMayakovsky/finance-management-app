using System.Net;

namespace FinanceManagement.Core.Exceptions
{
    public class ForbiddenException : HttpStatusCodeException
    {
        public ForbiddenException() : base(HttpStatusCode.Forbidden, null, ExceptionLogSeverity.Debug)
        {
        }

        public ForbiddenException(string msg) : base(HttpStatusCode.Forbidden, msg, ExceptionLogSeverity.Debug)
        {
        }
    }
}
