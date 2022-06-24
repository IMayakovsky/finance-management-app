using System;

namespace FinanceManagement.Core.Exceptions
{
    public class BaseException : Exception
    {
        public ExceptionLogSeverity Severity { get; }

        /// <summary>
        /// Message which will can be shown to the user.
        /// </summary>
        public string UserFriendlyMessage { get; set; }

        public BaseException(string message, Exception innerException, ExceptionLogSeverity severity) : base(message, innerException)
        {
            this.Severity = severity;
        }

        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
            this.Severity = ExceptionLogSeverity.Error;
        }

        public BaseException(string message) : base(message)
        {
            this.Severity = ExceptionLogSeverity.Error;
        }

        public BaseException SetUserFriendlyMessage(string visibleMessage)
        {
            UserFriendlyMessage = visibleMessage;
            return this;
        }
    }
}
