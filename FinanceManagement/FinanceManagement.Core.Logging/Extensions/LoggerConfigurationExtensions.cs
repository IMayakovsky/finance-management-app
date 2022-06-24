using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using FinanceManagement.Core.Logging.Common;

namespace FinanceManagement.Core.Logging.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration Console(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            loggerConfiguration.WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeyConsole}{LoggingConstants.KeyMinLogLevel}"])
                );

            return loggerConfiguration;
        }

        public static LoggerConfiguration Debug(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            loggerConfiguration.WriteTo.Debug(
                restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeyDebug}{LoggingConstants.KeyMinLogLevel}"])
                );

            return loggerConfiguration;
        }

        public static LoggerConfiguration FileAsync(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            loggerConfiguration.WriteTo.Async(sinkConfiguration =>
                    sinkConfiguration.File(
                        path: configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeyFilePath}"],
                        restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeyFile}{LoggingConstants.KeyMinLogLevel}"]),
                        buffered: true,
                        rollingInterval: RollingInterval.Day
                ));

            return loggerConfiguration;
        }

        public static LoggerConfiguration Seq(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            var minLogLevel = Enum.Parse<LogEventLevel>(
                    configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeySeq}{LoggingConstants.KeyMinLogLevel}"]);
            string c = configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeySeqUrl}"];
            loggerConfiguration.WriteTo.Seq(
                serverUrl: configuration[$"{LoggingConstants.KeyLoggingSettings}:{LoggingConstants.KeySeqUrl}"],
                restrictedToMinimumLevel: minLogLevel
            );

            return loggerConfiguration;
        }

        public static LoggerConfiguration ApplicationEnrich(this LoggerConfiguration loggerConfiguration, string environmentName)
        {
            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithAssemblyInfo()
                .Enrich.WithUserInfo()
                .Enrich.WithEnvironmentInfo(environmentName)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);

            return loggerConfiguration;
        }
    }
}
