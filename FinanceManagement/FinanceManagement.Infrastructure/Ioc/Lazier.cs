using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace FinanceManagement.Infrastructure.Ioc
{
    /// <summary>
    /// Helper class to hook up dependency injection with Lazy T
    /// </summary>
    [DebuggerStepThrough]
    internal class Lazier<T> : Lazy<T> where T : class
    {
        [DebuggerStepThrough]
        public Lazier(IServiceProvider provider) : base(() => provider.GetRequiredService<T>())
        {
        }
    }
}
