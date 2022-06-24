using Serilog.Core;
using Serilog.Events;

namespace FinanceManagement.Core.Logging.Enrichers
{
    public class UserInfoEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            string user = Thread.CurrentPrincipal?.Identity?.Name ?? $"{Environment.UserDomainName}\\{Environment.UserName}";

            logEvent.AddPropertyIfAbsent(new LogEventProperty("UserName", new ScalarValue(user)));
        }
    }
}
