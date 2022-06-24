using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace FinanceManagement.Infrastructure.Benchmarks
{
    // REMINDER: dont forget to run this app in Release mode outside DEBUG
    class Program
    {
        static void Main(string[] args)
        {
            // uncomment following line to run a specific benchmark
            //Summary summary = BenchmarkRunner.Run<DashboardBenchmark>();

            // this allows us to choose benchmark in the console app
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
