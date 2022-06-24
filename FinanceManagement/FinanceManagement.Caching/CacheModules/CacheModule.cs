using FinanceManagement.Caching;
using FinanceManagement.Core.Caching.Invalidation;
using FinanceManagement.Core.Collections;
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
using Polly;
using Polly.Retry;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Caching.CacheModules;

public interface ICacheModule
{
    void Run();

    void Stop();

    bool HandleDataCacheNotification(DataCacheNotification notification);

    /// <summary>
    /// Number of items in queue for loading
    /// </summary>
    int QueueSize { get; }

    /// <summary>
    /// Number of items in queue for loading, takes only items created when the cache modules were initialized (when application started) - initial cache preload
    /// </summary>
    int QueueSizeFromCacheModuleInitialization { get; }
}

public abstract class CachePreloadRequest
{
    public bool IsFromCacheModuleInitialization { get; set; }
}

public enum RequestPriority
{
    HighPriority = 0,
    LowPriority = 1,
}

public abstract class CacheModule : ICacheModule
{
    protected readonly IMemoryCache cache;
    protected readonly IServiceScopeFactory scopeFactory;
    protected readonly AsyncRetryPolicy initializePolicy;
    protected readonly AsyncPolicy processPreloadPolicy;

    private readonly AsyncCollection<KeyValuePair<int, CachePreloadRequest>> queue;
    private int queueSize;
    private int queueSizeOfItemsFromCacheModulesInitialization;

    private readonly CancellationTokenSource cts;
    protected CancellationToken Token => cts.Token;

    public CacheModule(IMemoryCache cache, IServiceScopeFactory scopeFactory)
    {
        this.cache = cache;
        this.scopeFactory = scopeFactory;

        cts = new CancellationTokenSource();
        queue = new AsyncCollection<KeyValuePair<int, CachePreloadRequest>>(new SimplePriorityQueue<int, CachePreloadRequest>(Enum.GetValues<RequestPriority>().Length));
        queueSize = 0;

        initializePolicy = Policy.Handle<Exception>().WaitAndRetryForeverAsync(
            (retryAttempt, context) => TimeSpan.FromSeconds(1),
            (exception, span, arg3) => Log.Error(exception, "Exception in CacheModule, gonna try again in 1 second."));

        processPreloadPolicy = Policy.Handle<Exception>().RetryAsync(3);
    }

    public void Run()
    {
        Task.Run(() => RunInternal());
    }

    public async Task RunInternal()
    {
        try
        {
            await initializePolicy.ExecuteAsync(InitAsync);

            if (PreloadsData)
            {
                for (int i = 0; i < PreloadThreadsCount; i++)
                {
                    _ = Task.Run(() => RunCachePreloader());
                }
            }
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Unhandled exception in {Name}", nameof(CacheModule));
            throw;
        }
    }

    public void Stop()
    {
        cts.Cancel();
    }

    protected async Task RunCachePreloader()
    {
        while (!Token.IsCancellationRequested)
        {
            try
            {
                var request = await queue.TakeAsync(Token);
                Interlocked.Decrement(ref queueSize);
                if (request.Value.IsFromCacheModuleInitialization)
                {
                    Interlocked.Decrement(ref queueSizeOfItemsFromCacheModulesInitialization);
                }

                try
                {
                    await processPreloadPolicy.ExecuteAsync(() => ProcessPreloadRequest(request.Value));
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error while processing CachePreload request {Request} of priority {Priority}",
                        request.Value, request.Key);
                }
            }
            catch (TaskCanceledException canceledException)
            {
                Log.Information(canceledException, "PreloadModule queue processing canceled.");
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in PreloadModule consumer. Execution will continue in 100ms.");
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
        }
    }

    protected abstract Task ProcessPreloadRequest(CachePreloadRequest request);

    protected void AddPreloadRequest(RequestPriority priority, CachePreloadRequest request)
    {
        queue.Add(new KeyValuePair<int, CachePreloadRequest>((int)priority, request));
        Interlocked.Increment(ref queueSize);
        if (request.IsFromCacheModuleInitialization)
        {
            Interlocked.Increment(ref queueSizeOfItemsFromCacheModulesInitialization);
        }
    }

    protected void StartPeriodicReload(TimeSpan timeSpan)
    {
        Task.Run(() => StartPeriodicReloadInternal(timeSpan, PeriodicReload));
    }

    protected void StartPeriodicReload(TimeSpan timeSpan, Func<Task> periodicReloadFunc)
    {
        Task.Run(() => StartPeriodicReloadInternal(timeSpan, periodicReloadFunc));
    }

    private async Task StartPeriodicReloadInternal(TimeSpan timeSpan, Func<Task> periodicReloadFunc)
    {
        while (!Token.IsCancellationRequested)
        {
            await Task.Delay(timeSpan);
            await periodicReloadFunc();
        }
    }

    protected virtual bool PreloadsData => true;
    protected virtual int PreloadThreadsCount => 1;

    public int QueueSize => queueSize;
    public int QueueSizeFromCacheModuleInitialization => queueSizeOfItemsFromCacheModulesInitialization;

    protected virtual Task PeriodicReload() { return Task.CompletedTask; }
    public abstract Task InitAsync();

    public abstract bool HandleDataCacheNotification(DataCacheNotification notification);
}
