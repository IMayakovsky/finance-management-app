using FinanceManagement.Core.Logging.Common;
using FinanceManagement.Core.Logging.Extensions;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Newton.N2.Core.Logging.Builder
{
    public interface ISerilogConfigurationBuilder
    {
        void Configure(IConfiguration configuration, LoggerConfiguration loggerConfiguration, string environmentName);
    }

    /// <summary>
    /// Helper class for quick logger configuration
    /// </summary>
    public class SerilogConfigurationBuilder : ISerilogConfigurationBuilder
    {
        public void Configure(IConfiguration configuration, LoggerConfiguration loggerConfiguration, string environmentName)
        {
            if (configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeyConsole}"] == "true")
            {
                loggerConfiguration.Console(configuration);
            }

            if (configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeyDebug}"] == "true")
            {
                loggerConfiguration.Debug(configuration);
            }

            if (configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeyFile}"] == "true")
            {
                loggerConfiguration.FileAsync(configuration);
            }

            if (configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeySeq}"] == "true")
            {
                loggerConfiguration.Seq(configuration);
            }

            loggerConfiguration.ApplicationEnrich(environmentName);
        }
    }
}
