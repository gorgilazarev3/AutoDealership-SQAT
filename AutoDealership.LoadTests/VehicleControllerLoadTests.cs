using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoDealership.Controllers;
using static Abstracta.JmeterDsl.JmeterDsl;

namespace AutoDealership.LoadTests
{
    public class VehicleControllerLoadTests
    {
        //Load testing the details page for the specific vehicle with id 9
        [Fact]
        public void VehicleController_Specific_Vehicle_Load_Test()
        {
            var stats = TestPlan(
                ThreadGroup(5, 200,
                    HttpSampler("https://localhost:44378/Vehicle/Details/9")
                ),
                ResultsTreeVisualizer()
            ).Run();
            Assert.Equal(0, stats.Overall.ErrorsCount);
            Assert.Equal(1000, stats.Overall.SamplesCount);
            Assert.True(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(10));
        }

        //Load testing the page for getting all brands
        [Fact]
        public void VehicleController_All_Brands_Load_Test()
        {
            var stats = TestPlan(
                ThreadGroup(5, 200,
                    HttpSampler("https://localhost:44378/Vehicle/Brands")
                ),
                ResultsTreeVisualizer()
            ).Run();
            Assert.Equal(0, stats.Overall.ErrorsCount);
            Assert.Equal(1000, stats.Overall.SamplesCount);
            Assert.True(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(10));
        }

        //Load testing the page for getting all reservations
        [Fact]
        public void VehicleController_All_Reservations_Load_Test()
        {
            var stats = TestPlan(
                ThreadGroup(5, 200,
                    HttpSampler("https://localhost:44378/Vehicle/VehicleReservations")
                ),
                ResultsTreeVisualizer()
            ).Run();
            Assert.Equal(0, stats.Overall.ErrorsCount);
            Assert.Equal(1000, stats.Overall.SamplesCount);
            Assert.True(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(10));
        }
    }
}
