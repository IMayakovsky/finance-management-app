using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace FinanceManagement.Core.Logging.Enrichers
{
    public class AssemblyInfoEnricher : ILogEventEnricher
    {
        private static AssemblyName assembly = new AssemblyName((Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).FullName);

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty("Application", new ScalarValue(assembly.Name)));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("ApplicationVersion", new ScalarValue($"{assembly.Version.Major}.{assembly.Version.Minor}.{assembly.Version.Build}.{assembly.Version.Revision}")));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("ComputerName", new ScalarValue(Environment.MachineName)));
        }
    }
}
