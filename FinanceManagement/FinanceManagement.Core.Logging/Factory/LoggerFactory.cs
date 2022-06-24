using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;
using FinanceManagement.Core.Logging.Extensions;

namespace FinanceManagement.Core.Logging.Factory
{
    public interface ILoggerFactory
    {
        public ILogger SeqLogger();

        Logger BuildConsoleLogger(string environmentName);

        Logger BuildDebugLogger(string environmentName);
    }

    public class LoggerFactory : ILoggerFactory
    {
        private readonly IConfiguration configuration;
        private readonly string environmentName;

        public LoggerFactory(IConfiguration configuration, string environmentName)
        {
            this.configuration = configuration;
            this.environmentName = environmentName;
        }

        public ILogger SeqLogger()
        {
            return new LoggerConfiguration().Seq(configuration).ApplicationEnrich(environmentName).CreateLogger();
        }

        public Logger BuildConsoleLogger(string environmentName)
        {
            return new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .Enrich.FromLogContext()
                .Enrich.WithAssemblyInfo()
                .Enrich.WithUserInfo()
                .Enrich.WithEnvironmentInfo(environmentName)
                .CreateLogger();
        }

        public Logger BuildDebugLogger(string environmentName)
        {
            return new LoggerConfiguration()
                .WriteTo.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithAssemblyInfo()
                .Enrich.WithUserInfo()
                .Enrich.WithEnvironmentInfo(environmentName)
                .CreateLogger();
        }
    }
}
