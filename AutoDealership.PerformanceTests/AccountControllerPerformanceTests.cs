using AutoDealership.Controllers;
using NBench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDealership.PerformanceTests
{
    internal class AccountControllerPerformanceTests
    {
        Counter loginCounter;
        AccountController controller;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            controller = new AccountController();
            loginCounter = context.GetCounter("LoginCounter");
        }

        [PerfBenchmark(NumberOfIterations = 5,
            RunMode = RunMode.Throughput,
            RunTimeMilliseconds = 2000,
            TestMode = TestMode.Test)]
        [CounterThroughputAssertion("LoginCounter", MustBe.GreaterThanOrEqualTo, 1500)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, 256000)]
        public void AcountController_Login_Page_Performance_Test()
        {
            controller.Login("");
            loginCounter.Increment();
        }


        [PerfCleanup]
        public void Cleanup()
        {
            controller.Dispose();
        }
    }
}
