using Dawn;
using FinanceManagement.Core.Logging.Enrichers;
using Serilog;
using Serilog.Configuration;

namespace FinanceManagement.Core.Logging.Extensions
{
    public static class LoggingExtensions
    {
        public static LoggerConfiguration WithAssemblyInfo(this LoggerEnrichmentConfiguration enrich)
        {
            Guard.Argument(enrich, nameof(enrich)).NotNull();

            return enrich.With<AssemblyInfoEnricher>();
        }

        public static LoggerConfiguration WithUserInfo(this LoggerEnrichmentConfiguration enrich)
        {
            Guard.Argument(enrich, nameof(enrich)).NotNull();

            return enrich.With<UserInfoEnricher>();
        }

        public static LoggerConfiguration WithEnvironmentInfo(this LoggerEnrichmentConfiguration enrich, string environmentName)
        {
            Guard.Argument(enrich, nameof(enrich)).NotNull();

            EnvironmentEnricher.EnvironmentName = environmentName;

            return enrich.With<EnvironmentEnricher>();
        }
    }
}
