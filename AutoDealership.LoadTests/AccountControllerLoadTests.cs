using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Abstracta.JmeterDsl.JmeterDsl;

namespace AutoDealership.LoadTests
{
    public class AccountControllerLoadTests
    {
        //Load testing the login page where users need to login
        [Fact]
        public void AccountController_Login_Page_Load_Test()
        {
            var stats = TestPlan(
                ThreadGroup(5, 200,
                    HttpSampler("https://localhost:44378/Account/Login")
                ),
                ResultsTreeVisualizer()
            ).Run();
            Assert.Equal(0, stats.Overall.ErrorsCount);
            Assert.Equal(1000, stats.Overall.SamplesCount);
            Assert.True(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(5));
        }

        //Load testing the register page where users need to register
        [Fact]
        public void AccountController_Register_Page_Load_Test()
        {
            var stats = TestPlan(
                ThreadGroup(5, 200,
                    HttpSampler("https://localhost:44378/Account/Register")
                ),
                ResultsTreeVisualizer()
            ).Run();
            Assert.Equal(0, stats.Overall.ErrorsCount);
            Assert.Equal(1000, stats.Overall.SamplesCount);
            Assert.True(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(5));
        }
    }
}
