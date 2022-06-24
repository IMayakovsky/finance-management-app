using FinanceManagement.MessageService.Processors;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.MessageService
{
    internal class MessageServiceProcess
    {
        private readonly IServiceProvider serviceProvider;

        private readonly List<BaseProccessor> proccessors;

        public MessageServiceProcess(IServiceCollection services)
        {
            services.AddScoped<BaseProccessor, EmailProccessor>();

            serviceProvider = services.BuildServiceProvider();

            proccessors = serviceProvider.GetServices<BaseProccessor>().ToList();
        }

        public async Task Process()
        {
            foreach (var processor in proccessors)
            {
                Log.Information($"{processor.GetType().Name} Processing");

                await processor.Process();
            }
        }
    }
}
