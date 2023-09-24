using AutoDealership.Controllers;
using NBench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDealership.PerformanceTests
{
    internal class HomeControllerPerformanceTests
    {
        Counter testCounter;
        HomeController controller;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            controller = new HomeController(new Models.ApplicationDbContext());
            testCounter = context.GetCounter("InventoryCounter");
        }

        [PerfBenchmark(NumberOfIterations = 5,
    RunMode = RunMode.Throughput,
    RunTimeMilliseconds = 2000,
    TestMode = TestMode.Test)]
        [CounterThroughputAssertion("InventoryCounter", MustBe.GreaterThanOrEqualTo, 1000)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, 256000)]
        public void HomeController_Inventory_List_Vehicles_Test()
        {
            controller.Inventory("m");
            testCounter.Increment();
        }


        [PerfCleanup]
        public void Cleanup()
        {
            controller.Dispose();
        }
    }
}
