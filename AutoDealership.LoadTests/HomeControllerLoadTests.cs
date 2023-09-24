using System;
using Xunit;
using Abstracta.JmeterDsl.BlazeMeter;
using AutoDealership.Controllers;
using static Abstracta.JmeterDsl.JmeterDsl;
namespace AutoDealership.LoadTests
{
    public class HomeControllerLoadTests
    {

        HomeController controller;

        public HomeControllerLoadTests()
        {
            this.controller = new HomeController(new Models.ApplicationDbContext());
        }


        //Load testing the home page that users access when they first go to the app
        [Fact]
        public void HomeController_Index_Load_Test()
        {
            var stats = TestPlan(
                // number of threads and iterations are in the end overwritten by BlazeMeter engine settings 
                ThreadGroup(5, 200,
                    HttpSampler("https://localhost:44378/")
                ),
                ResultsTreeVisualizer()
            ).Run();
            Assert.Equal(0, stats.Overall.ErrorsCount);
            Assert.Equal(1000, stats.Overall.SamplesCount);
            Assert.True(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(5));
        }

        //Load testing the inventory page that users access when they want to search for vehicles
        [Fact]
        public void HomeController_Inventory_Search_Load_Test()
        {
            var stats = TestPlan(
                // number of threads and iterations are in the end overwritten by BlazeMeter engine settings 
                ThreadGroup(5, 200,
                    HttpSampler("https://localhost:44378/Home/Inventory?search=golf")
                ),
                ResultsTreeVisualizer()
            ).Run();
            Assert.Equal(0, stats.Overall.ErrorsCount);
            Assert.Equal(1000, stats.Overall.SamplesCount);
            Assert.True(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(10));
        }

        //Load testing the inventory page that for all vehicles
        [Fact]
        public void HomeController_Inventory_Load_Test()
        {
            var stats = TestPlan(
                // number of threads and iterations are in the end overwritten by BlazeMeter engine settings 
                ThreadGroup(5, 200,
                    HttpSampler("https://localhost:44378/Home/Inventory")
                ),
                ResultsTreeVisualizer()
            ).Run();
            Assert.Equal(0, stats.Overall.ErrorsCount);
            Assert.Equal(1000, stats.Overall.SamplesCount);
            Assert.True(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(5));
        }



        //[Fact]
        //public void HomeController_Index_Load_Test2()
        //{
        //    var stats = TestPlan(
        //        // number of threads and iterations are in the end overwritten by BlazeMeter engine settings 
        //        ThreadGroup(2, 100,
        //            HttpSampler("https://localhost:44378/")
        //        ),
        //        ResultsTreeVisualizer()
        //    ).RunIn(new BlazeMeterEngine("613576bd29d6c6dd93a30281:8dd109530c2bb81f16203c98d5bc17f9edc524f6c86436565ea029bda123d3c565855db6")
        //        .TestName("Users access the home page")
        //        .TotalUsers(50)
        //        .ThreadsPerEngine(2)
        //        .TestName("homecontroller-index-test"));
        //    Assert.Equal(0, stats.Overall.ErrorsCount);
        //}
    }
}
