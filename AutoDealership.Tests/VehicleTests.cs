using AutoDealership.Controllers;
using AutoDealership.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace AutoDealership.Tests
{
    public class VehicleTests
    {
        [Fact]
        public void Vehicle_GetMPG_Test()
        {
            Vehicle veh = new Vehicle();
            //in l/100km
            veh.FuelEfficiency = 5;
            //the formula for calculating MPG is 235.214583 / FuelEfficiency , so the result should be
            //235.214583 / 5 = 47.0429166
            Assert.Equal(47.0429166, veh.GetMPG());
        }

        [Fact]
        public void Brand_ToString_Test()
        {
            Brand brand = new Brand();
            brand.Name = "Test";
            //Brand.ToString() returns the name of the brand, so it should be equal to 'Test'
            Assert.Equal("Test", brand.ToString());
        }

        [Fact]
        public void HomeController_VehicleToString_Test()
        {
            var dbContextMock = new Mock<ApplicationDbContext>();
            var vehiclesSetMock = new Mock<DbSet<Vehicle>>().SetupData(TestsHelpers.GetMockVehicleList());
            var brandsSetMock = new Mock<DbSet<Brand>>().SetupData(TestsHelpers.GetMockBrandsList());
            brandsSetMock.Setup(m => m.Find(It.IsAny<object[]>()))
    .Returns<object[]>(ids => TestsHelpers.GetMockBrandsList().FirstOrDefault(d => d.Id == (int)ids[0]));
            vehiclesSetMock.Setup(m => m.Find(It.IsAny<object[]>()))
.Returns<object[]>(ids => TestsHelpers.GetMockVehicleList().FirstOrDefault(d => d.Id == (int)ids[0]));
            dbContextMock.Setup(x => x.Vehicles).Returns(vehiclesSetMock.Object);
            dbContextMock.Setup(x => x.Brands).Returns(brandsSetMock.Object);

            Assert.Equal("2018 Mercedes E63s", HomeController.VehicleToStringWithContext(vehiclesSetMock.Object.First(), dbContextMock.Object));


        }
    }
}
