using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using System;
using System.Net;
using Xunit;
using Owin;
using System.Web;
using AutoDealership.Controllers;
using System.Threading.Tasks;
using AutoDealership.Models;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;

namespace AutoDealership.IntegrationTests
{
    [Collection("TestDatabase")]
    public class UsersVehiclesIntegrationTests
    {
        DatabaseFixture fixture;
        IAuthenticationManager authenticationManager;
        //private readonly IContextWrapper contextWrapper;
        VehicleController vehicleController;

        public UsersVehiclesIntegrationTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
            vehicleController = new VehicleController(fixture.context);
        }

        [Fact]
        public async Task Admin_User_Creates_Vehicle_And_Deletes_ItAsync()
        {
            var user = fixture.userManager.Find(TestConstants.UserEmail, TestConstants.UserPassword);
            Assert.NotNull(user);
            var model = new LoginViewModel 
            {
                Email = TestConstants.UserEmail,
                Password = TestConstants.UserPassword,
                RememberMe = false
            };
            //Checking if the user is authenticated and that it is in the correct role
            //so he can create a vehicle listing
            var identity = await fixture.userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            var contextUser = new ClaimsPrincipal(identity);
            Assert.True(contextUser.Identity.IsAuthenticated);
            Assert.True(contextUser.IsInRole(TestConstants.UserAdminRole));
            //Let's create a new vehicle listing and verify that it is there
            var brand = fixture.context.Brands.FirstOrDefault();
            var vehicleToAdd = new Vehicle
            {
                BrandId = brand.Id,
                Model = "E220d",
                FuelType = Fuel.DIESEL,
                Features = "Test",
                BodyStyle = Models.Types.BodyStyle.CONVERTIBLE,
                Price = 50000,
                Color = "White",
                Horsepower = 193,
                Year = 2017,
                Transmission = new Models.Types.Transmission { NumberSpeeds = 7, TransmissionType = Models.Types.TransmissionType.AUTOMATIC}
            };
            //Let's get the count of the vehicles before adding
            int countVehicles = fixture.context.Vehicles.ToList().Count;
            var resultCreate = vehicleController.Create(vehicleToAdd) as ViewResult;
            //Wait half a second for the record to be saved in the database
            Thread.Sleep(500);
            //Since we have only one vehicle added into the testing database, the new vehicle should be adding one to the collection
            Assert.Equal(countVehicles + 1, fixture.context.Vehicles.ToList().Count);
            //also let's test that it contains the vehicle by model and id
            Assert.Contains(vehicleToAdd, fixture.context.Vehicles.ToList());
            //Let's now test by getting it from the details page
            //since it's the second vehicle the ID should be 1
            int vehId = fixture.context.Vehicles.Where(v => v.Model.Equals(vehicleToAdd.Model)).ToList().FirstOrDefault().Id;
            var resultDetails = vehicleController.Details(vehId) as ViewResult;
            var vehicleDetails = resultDetails.Model as VehicleDetailsViewModel;
            Assert.NotNull(vehicleDetails);
            Assert.Equal(vehId, vehicleDetails.Vehicle.Id);
            Assert.Equal("E220d", vehicleDetails.Vehicle.Model);
            //Now let's delete the vehicle and test that it is not there
            var resultDelete = vehicleController.DeleteConfirmed(vehId) as ViewResult;
            Assert.DoesNotContain(vehicleToAdd, fixture.context.Vehicles.ToList());
        }

        [Fact]
        public async Task Admin_User_Creates_Brand_And_Deletes_ItAsync()
        {
            var user = fixture.userManager.Find(TestConstants.UserEmail, TestConstants.UserPassword);
            Assert.NotNull(user);
            var model = new LoginViewModel
            {
                Email = TestConstants.UserEmail,
                Password = TestConstants.UserPassword,
                RememberMe = false
            };
            //Checking if the user is authenticated and that it is in the correct role
            //so he can create a new brand
            var identity = await fixture.userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            var contextUser = new ClaimsPrincipal(identity);
            Assert.True(contextUser.Identity.IsAuthenticated);
            Assert.True(contextUser.IsInRole(TestConstants.UserAdminRole));
            //Let's create a new brand and verify that it is there

            var brand = new Brand
            {
                Name = "BMW",
                Vehicles = new List<Vehicle>()
            };
            var createBrandModel = new CreateVehicleViewModel
            {
                NewBrand = brand
            };
            //Let's get the count of the brands before adding
            int countBrands = fixture.context.Brands.ToList().Count;
            var resultCreate = vehicleController.CreateBrand(createBrandModel) as ViewResult;
            //Wait half a second for the record to be saved in the database
            Thread.Sleep(500);
            //Since we have only one brand added into the testing database, the new brand should be adding one to the collection
            Assert.Equal(countBrands + 1, fixture.context.Brands.ToList().Count);
            //also let's test that it contains the brand by model and id
            Assert.Contains(brand, fixture.context.Brands.ToList());
        }
    }
}
