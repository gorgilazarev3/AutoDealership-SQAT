using AutoDealership.Controllers;
using AutoDealership.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Xunit;

namespace AutoDealership.Tests
{
    public class HomeControllerTests
    {

        Mock<ApplicationDbContext> dbContextMock;
        Mock<DbSet<Vehicle>> vehiclesSetMock;
        Mock<DbSet<Brand>> brandsSetMock;
        HomeController controller;

        public HomeControllerTests()
        {
            //set up the mocks
            dbContextMock = new Mock<ApplicationDbContext>();
            vehiclesSetMock = new Mock<DbSet<Vehicle>>().SetupData(TestsHelpers.GetMockVehicleList());
            brandsSetMock = new Mock<DbSet<Brand>>().SetupData(TestsHelpers.GetMockBrandsList());
            brandsSetMock.Setup(m => m.Find(It.IsAny<object[]>()))
    .Returns<object[]>(ids => TestsHelpers.GetMockBrandsList().FirstOrDefault(d => d.Id == (int)ids[0]));
            vehiclesSetMock.Setup(m => m.Find(It.IsAny<object[]>()))
.Returns<object[]>(ids => TestsHelpers.GetMockVehicleList().FirstOrDefault(d => d.Id == (int)ids[0]));
            dbContextMock.Setup(x => x.Vehicles).Returns(vehiclesSetMock.Object);
            dbContextMock.Setup(x => x.Brands).Returns(brandsSetMock.Object);
            //set up the controller with the mock database
            controller = new HomeController(dbContextMock.Object);
        }

        [Fact]
        public void HomeController_Index_Returns_All_Vehicles_And_Top_5_Brands()
        {

            var result = (controller.Index()) as ViewResult;
            var vehicleList = (List<Vehicle>)result?.ViewData.Model;
            var brandsList = (List<Brand>)result?.ViewData["Brands"];
            Assert.Equal(2, vehicleList.Count);
            //If the mock list has less than 5 items check if the count is equal to the number of items
            if(TestsHelpers.GetMockBrandsList().Count >= 5)
            {
                Assert.Equal(5, brandsList.Count);
            }
            else
            {
                Assert.Equal(TestsHelpers.GetMockBrandsList().Count, brandsList.Count);
            }

        }

        [Fact]
        public void HomeController_Inventory_Returns_Correct_Vehicle_With_Multiple_Parameters()
        {

            //The inventory function is for searching the available vehicles by all parameters:
            //vehicle model, year, brand name etc...
            //Let's test by vehicle brand and name and year, example Mercedes e63s 2018 should return one item
            //therefore the min price should be the price and the max price should be the min price + 100

            string query = "mercedes e63s 2018";
            var result = (controller.Inventory(query)) as ViewResult;
            var mercedesVeh = TestsHelpers.GetMockVehicleList().Where(v => v.Model.ToLower().Contains("e63s")).First();
            Assert.Equal(mercedesVeh.Price, result.ViewBag.MinPrice);
            Assert.Equal(mercedesVeh.Price + 100, result.ViewBag.MaxPrice);
            var model = result.ViewData.Model as InventoryViewModel;
            Assert.Single(model.Inventory);
            Assert.Single(model.FullInventory);
            Assert.Equal(TestsHelpers.GetMockBrandsList().Count, model.AllBrands.Count);
            //there should be one page only since we have only one item and the active page should be 0
            //since the active pages are 0 based to help with deciding the count of pages
            Assert.Equal(1, model.Pages);
            Assert.Equal(0, model.ActivePage);

        }

        [Fact]
        public void HomeController_Inventory_Returns_Correct_Vehicle_With_One_Parameter()
        {

            //The inventory function is for searching the available vehicles by one parameter:
            //Let's test by the letter m, example by brand mercedes and bmw will both be chosen, also by the model name m5 in bmw, so the function should return two items
            //therefore the min price should be the price of the mercedes and the max price should be the price of the bmw + 100 since it's more expensive

            string query = "m";
            var result = (controller.Inventory(query)) as ViewResult;
            var mercedesVeh = TestsHelpers.GetMockVehicleList().Where(v => v.Model.ToLower().Contains("e63s")).First();
            var bmwVeh = TestsHelpers.GetMockVehicleList().Where(v => v.Model.ToLower().Contains("m5")).First();
            Assert.Equal(mercedesVeh.Price, result.ViewBag.MinPrice);
            Assert.Equal(bmwVeh.Price + 100, result.ViewBag.MaxPrice);
            var model = result.ViewData.Model as InventoryViewModel;
            Assert.Equal(TestsHelpers.GetMockVehicleList().Count, model.Inventory.Count);
            Assert.Equal(TestsHelpers.GetMockVehicleList().Count, model.FullInventory.Count);
            Assert.Equal(TestsHelpers.GetMockBrandsList().Count, model.AllBrands.Count);
            //there should be one page only since we have only two items and the active page should be 0
            //since the active pages are 0 based to help with deciding the count of pages
            Assert.Equal(1, model.Pages);
            Assert.Equal(0, model.ActivePage);
        }

        [Fact]
        public void HomeController_InventoryByBodyStyle_Returns_Correct_Vehicles()
        {

            //The InventoryByBodyStyle function return the vehicles by the selected body style
            //since we only have sedans in the mock database, we will search by body style sedan
            //the expected result should have the two vehicles that are sedans

            string query = "sedan";
            var result = (controller.InventoryByBodyStyle(query)) as ViewResult;
            var model = result.ViewData.Model as InventoryViewModel;
            Assert.Equal(TestsHelpers.GetMockVehicleList().Count, model.Inventory.Count);
            Assert.Equal(TestsHelpers.GetMockVehicleList().Count, model.FullInventory.Count);
            Assert.Equal(TestsHelpers.GetMockBrandsList().Count, model.AllBrands.Count);
            //there should be one page only since we have only two items and the active page should be 0
            //since the active pages are 0 based to help with deciding the count of pages
            Assert.Equal(1, model.Pages);
            Assert.Equal(0, model.ActivePage);
        }

        [Fact]
        public void HomeController_InventoryByPrice_Returns_Correct_Vehicles()
        {

            //The InventoryByPrice function returns the vehicles that have a price of less than the given price
            //we will enter the value of 110000 and since we have only two vehicles,
            //the one from 100000$ should be returned

            int price = 110000;
            var result = (controller.InventoryByPrice(price, "")) as ViewResult;
            var model = result.ViewData.Model as InventoryViewModel;
            Assert.Single(model.Inventory);
            Assert.Single(model.FullInventory);
            Assert.Equal(TestsHelpers.GetMockBrandsList().Count, model.AllBrands.Count);
            //there should be one page only since we have only one item and the active page should be 0
            //since the active pages are 0 based to help with deciding the count of pages
            Assert.Equal(1, model.Pages);
            Assert.Equal(0, model.ActivePage);
        }

        [Fact]
        public void HomeController_SpeedInventory_Returns_Correct_Vehicles()
        {

            //The SpeedInventory function returns the vehicles that have more than 550 horsepower and belong in the sportscar category
            //since both our cars have more than 550 horsepower, both should be returned

            var result = (controller.SpeedInventory()) as ViewResult;
            var model = result.ViewData.Model as InventoryViewModel;
            Assert.Equal(TestsHelpers.GetMockVehicleList().Count, model.Inventory.Count);
            Assert.Equal(TestsHelpers.GetMockVehicleList().Count, model.FullInventory.Count);
            Assert.Equal(TestsHelpers.GetMockBrandsList().Count, model.AllBrands.Count);
            //there should be one page only since we have only one item and the active page should be 0
            //since the active pages are 0 based to help with deciding the count of pages
            Assert.Equal(1, model.Pages);
            Assert.Equal(0, model.ActivePage);
        }
    }
}
