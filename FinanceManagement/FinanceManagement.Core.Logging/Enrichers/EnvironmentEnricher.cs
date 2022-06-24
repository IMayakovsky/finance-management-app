using Serilog.Core;
using Serilog.Events;

namespace FinanceManagement.Core.Logging.Enrichers
{
    public class EnvironmentEnricher : ILogEventEnricher
    {
        public static string EnvironmentName = "Unknown";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty("Environment", new ScalarValue(EnvironmentName)));
        }
    }
}
