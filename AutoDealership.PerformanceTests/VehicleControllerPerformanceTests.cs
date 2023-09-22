using AutoDealership.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBench;

namespace AutoDealership.PerformanceTests
{
    internal class VehicleControllerPerformanceTests
    {
        Counter testCounter;
        VehicleController controller;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            controller = new VehicleController(new Models.ApplicationDbContext());
            testCounter = context.GetCounter("ListVehiclesCounter");
        }

        [PerfBenchmark(NumberOfIterations = 5,
            RunMode = RunMode.Throughput,
            RunTimeMilliseconds = 2000,
            TestMode = TestMode.Test)]
        [CounterThroughputAssertion("ListVehiclesCounter", MustBe.GreaterThanOrEqualTo, 1500)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, 256000)]
        public void VehicleController_Index_List_Vehicles_Test()
        {
            controller.Index();
            testCounter.Increment();
        }

        [PerfCleanup]
        public void Cleanup() 
        {
            controller.Dispose();
        }
    }
}
