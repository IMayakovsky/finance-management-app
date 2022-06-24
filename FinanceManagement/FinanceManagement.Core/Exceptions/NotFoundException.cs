using System.Net;

namespace FinanceManagement.Core.Exceptions
{
    public class NotFoundException : HttpStatusCodeException
    {
        public NotFoundException() : base(HttpStatusCode.NotFound, null, ExceptionLogSeverity.Debug)
        {
        }

        public NotFoundException(string msg) : base(HttpStatusCode.NotFound, msg, ExceptionLogSeverity.Debug)
        {
        }
    }
}
