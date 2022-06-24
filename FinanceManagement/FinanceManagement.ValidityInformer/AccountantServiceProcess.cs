using FinanceManagement.ValidityInformer.Processors;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.ValidityInformer
{
    internal class AccountantServiceProcess
    {
        private readonly IServiceProvider serviceProvider;

        private readonly List<BaseProccessor> proccessors;

        public AccountantServiceProcess(IServiceCollection services)
        {
            services.AddScoped<BaseProccessor, SubscriptionValidateProccessor>();
            services.AddScoped<BaseProccessor, DebtProccessor>();
            services.AddScoped<BaseProccessor, SubscriptionBillingProccessor>();

            serviceProvider = services.BuildServiceProvider();

            proccessors = serviceProvider.GetServices<BaseProccessor>().ToList();
        }

        public async Task Validate()
        {
            foreach (var processor in proccessors)
            {
                Log.Information($"{processor.GetType().Name} Processing");

                await processor.Process();
            }
        }

        public void ValidateProcess()
        {

        }
    }
}
