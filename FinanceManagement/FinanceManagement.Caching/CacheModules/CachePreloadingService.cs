using FinanceManagement.Core.Caching.Invalidation;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Caching.CacheModules;

public class CachePreloadingService : IHostedService, IDisposable
{
    private readonly List<ICacheModule> cacheModules = new List<ICacheModule>();

    public void AddCacheModule(ICacheModule module)
    {
        cacheModules.Add(module);
        module.Run();
    }

    public List<ICacheModule> CacheModules => cacheModules;

    public Task StartAsync(CancellationToken stoppingToken)
    {
        Log.Information("Cache Preloading Hosted Service is running.");

        return Task.CompletedTask;
    }

    public void HandleDataCacheNotification(DataCacheNotification notification)
    {
        foreach (var module in CacheModules)
        {
            if (module.HandleDataCacheNotification(notification))
            {
                // notification was handled by the module
                break;
            }
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        Log.Information("Cache Preloading Hosted Service is stopping.");

        foreach (var module in CacheModules)
        {
            module.Stop();
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}
