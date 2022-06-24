using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newton.N2.Core.Logging.Builder;
using Serilog;

namespace Finance_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) =>
                {
                    new SerilogConfigurationBuilder().Configure(context.Configuration, configuration, context.HostingEnvironment.EnvironmentName);

                    // for web app - we also want to correlate requests
                    configuration.Enrich.WithCorrelationId();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
