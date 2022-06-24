using System;
using System.Net;

namespace FinanceManagement.Core.Exceptions
{
    public class HttpStatusCodeException : BaseException
    {
        public HttpStatusCode Status { get; }

        public HttpStatusCodeException(HttpStatusCode status, Exception innerException, string msg, ExceptionLogSeverity severity = ExceptionLogSeverity.Error) : base(msg, innerException, severity)
        {
            this.Status = status;
        }

        public HttpStatusCodeException(HttpStatusCode status, string msg, ExceptionLogSeverity severity = ExceptionLogSeverity.Error) : base(msg, null, severity)
        {
            this.Status = status;
        }
    }
}
