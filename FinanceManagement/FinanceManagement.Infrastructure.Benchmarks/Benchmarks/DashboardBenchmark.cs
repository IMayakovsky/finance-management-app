using BenchmarkDotNet.Attributes;
using FinanceManagement.Infrastructure.Operations.Transients;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManagement.Infrastructure.Benchmarks;

[SimpleJob]
public class DashboardBenchmark : BaseBenchmark
{
    private readonly int userId = 1;
    private readonly int accountId = 1;
    private readonly int count = 50;

    [GlobalSetup]
    public void Setup()
    {
        // for all data loading from cache
        using (var scope = Services.CreateScope())
        {
            var dashboardOperation = scope.ServiceProvider.GetService<IDashboardOperation>();

            dashboardOperation.GetTopEntities(userId, accountId, count).Wait();
        }
    }

    [Benchmark]
    public async Task GetTopEntitiesBenchmark()
    {
        using (var scope = Services.CreateScope())
        {
            var dashboardOperation = scope.ServiceProvider.GetService<IDashboardOperation>();
            await dashboardOperation.GetTopEntities(userId, accountId, count);
        }
    }
}
