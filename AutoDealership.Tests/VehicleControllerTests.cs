using AutoDealership.Controllers;
using AutoDealership.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace AutoDealership.Tests
{
    public class VehicleControllerTests
    {
        Mock<ApplicationDbContext> dbContextMock;
        Mock<DbSet<Vehicle>> vehiclesSetMock;
        Mock<DbSet<Brand>> brandsSetMock;
        Mock<DbSet<ApplicationUser>> usersSetMock;
        Mock<DbSet<VehicleReservation>> vehicleReservationsSetMock;
        VehicleController controller;

        public VehicleControllerTests()
        {
            //set up the mocks
            dbContextMock = new Mock<ApplicationDbContext>();
            vehiclesSetMock = new Mock<DbSet<Vehicle>>().SetupData(TestsHelpers.GetMockVehicleList());
            brandsSetMock = new Mock<DbSet<Brand>>().SetupData(TestsHelpers.GetMockBrandsList());
            brandsSetMock.Setup(m => m.Find(It.IsAny<object[]>()))
    .Returns<object[]>(ids => TestsHelpers.GetMockBrandsList().FirstOrDefault(d => d.Id == (int)ids[0]));
            vehiclesSetMock.Setup(m => m.Find(It.IsAny<object[]>()))
.Returns<object[]>(ids => TestsHelpers.GetMockVehicleList().FirstOrDefault(d => d.Id == (int)ids[0]));
            usersSetMock = new Mock<DbSet<ApplicationUser>>().SetupData(TestsHelpers.GetMockUserList());
            vehicleReservationsSetMock = new Mock<DbSet<VehicleReservation>>().SetupData(TestsHelpers.GetMockReservationsList());
            usersSetMock.Setup(m => m.Find(It.IsAny<object[]>()))
    .Returns<object[]>(ids => TestsHelpers.GetMockUserList().FirstOrDefault(d => d.Id.Equals((string)ids[0])));
            vehicleReservationsSetMock.Setup(m => m.Find(It.IsAny<object[]>()))
.Returns<object[]>(ids => TestsHelpers.GetMockReservationsList().FirstOrDefault(d => d.Id == (int)ids[0]));
            dbContextMock.Setup(x => x.Vehicles).Returns(vehiclesSetMock.Object);
            dbContextMock.Setup(x => x.Brands).Returns(brandsSetMock.Object);
            dbContextMock.Setup(x => x.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(x => x.VehicleReservations).Returns(vehicleReservationsSetMock.Object);
            dbContextMock
    .Setup(m => m.Vehicles.Remove(It.IsAny<Vehicle>()))
    .Callback<Vehicle>((entity) => TestsHelpers.GetMockVehicleList().Remove(entity));
           vehiclesSetMock
.Setup(m => m.Remove(It.IsAny<Vehicle>()))
.Callback<Vehicle>((entity) => TestsHelpers.GetMockVehicleList().Remove(entity));
            //set up the controller with the mock database
            controller = new VehicleController(dbContextMock.Object);
        }

        [Fact]
        public void VehicleController_Index_Model_Contains_All_Brands_And_Vehicles()
        {
            //We need to check that the model returned in Index contains all vehicles
            //and also the viewbag contains all brands
            var result = controller.Index() as ViewResult;
            var model = result.ViewData.Model as List<Vehicle>;
            var brands = result.ViewBag.Brands as List<Brand>;
            Assert.Equal(TestsHelpers.GetMockVehicleList().Count, model.Count);
            Assert.Equal(TestsHelpers.GetMockBrandsList().Count, brands.Count);
            

        }

        [Fact]
        public void VehicleController_Details_Returns_Correct_Vehicle_And_Brand()
        {
            //We need to check that the function returns the correct vehicle and brand of the vehicle
            //for the given id, in this test i will chose id 1 - that is the mercedes vehicle
            int id = 1;
            var result = controller.Details(id) as ViewResult;
            var model = result.ViewData.Model as VehicleDetailsViewModel;
            var vehicle = vehiclesSetMock.Object.Find(id);
            var brand = brandsSetMock.Object.Find(vehicle.BrandId);
            var vehicleJson = JsonConvert.SerializeObject(vehicle);
            var returnedVehicleJson = JsonConvert.SerializeObject(model.Vehicle);
            Assert.Equal(vehicleJson, returnedVehicleJson);
            var brandJson = JsonConvert.SerializeObject(brand);
            var returnedBrandJson = JsonConvert.SerializeObject(model.Brand);
            Assert.Equal(brandJson, returnedBrandJson);

        }

        [Fact]
        public void VehicleController_Create_Successfully_Adds_Vehicle()
        {
            var vehicleToAdd = new Vehicle
            {
                Id = 3,
                BrandId = 1,
                Model = "E220d",
                FuelType = Fuel.DIESEL,
                Features = "Test",
                BodyStyle = Models.Types.BodyStyle.CONVERTIBLE,
                Price = 50000,
                Color = "White",
                Horsepower = 193,
                Year = 2017
            };
            var result = controller.Create(vehicleToAdd);
            //let's test that the collection has 1 more item than the method used to set up the mock dbset for vehicles
            Assert.Equal(TestsHelpers.GetMockVehicleList().Count + 1, dbContextMock.Object.Vehicles.ToList().Count);
            //also let's test that it contains the vehicle by model and id
            Assert.Contains(vehicleToAdd, dbContextMock.Object.Vehicles.ToList());
        }

        [Fact]
        public void VehicleController_CreateBrand_Successfully_Adds_Brand()
        {
            var brand = new Brand
            {
                Id = 3,
                Name = "Volkswagen",
                Vehicles = new List<Vehicle>()
            };
        var model = new CreateVehicleViewModel 
            {
            NewBrand = brand
            };

            var result = controller.CreateBrand(model);
            //let's test that the collection has 1 more item than the method used to set up the mock dbset for brands
            Assert.Equal(TestsHelpers.GetMockBrandsList().Count + 1, dbContextMock.Object.Brands.ToList().Count);
            //also let's test that it contains the brand by model and id
            Assert.Contains(brand, dbContextMock.Object.Brands.ToList());
        }

        [Fact]
        public void VehicleController_GetBrand_Returns_Correct_Brand()
        {
            var result = controller.GetBrand(1) as JsonResult;
            var brand = brandsSetMock.Object.Find(1);
            var brandJson = JsonConvert.SerializeObject(brand);
            var returnedBrandJson = JsonConvert.SerializeObject(result.Data);
            Assert.Equal(brandJson, returnedBrandJson);
        }

        [Fact]
        public void VehicleController_Brands_Returns_All_Brands()
        {
            var result = controller.Brands() as ViewResult;
            var model = result.Model as List<Brand>;
            Assert.Equal(TestsHelpers.GetMockBrandsList().Count, model.Count);
        }


        [Fact]
        public void VehicleController_VehicleReservations_Returns_All_Reservations()
        {
            var result = controller.VehicleReservations() as ViewResult;
            var model = result.Model as List<VehicleReservation>;
            var users = result.ViewBag.Users;
            var vehicles = result.ViewBag.Vehicles;
            Assert.Equal(TestsHelpers.GetMockReservationsList().Count, model.Count);
            Assert.Equal(TestsHelpers.GetMockUserList().Count, users.Count);
            Assert.Equal(TestsHelpers.GetMockVehicleList().Count, vehicles.Count);
        }

        [Fact]
        public void VehicleController_ReserveVehicle_Successfully_Reserves_Vehicle()
        {
            var reservation = new VehicleReservation 
            {

                    Id = 2,
                    IsTestDrive = false,
                    ReservedUntil = DateTime.Now,
                    UserId = "Test",
                    VehicleId = 1

            };
            var result = controller.ReserveVehicle(reservation);
            //let's test that the collection has 1 more item than the method used to set up the mock dbset for reservations
            Assert.Equal(TestsHelpers.GetMockReservationsList().Count + 1, dbContextMock.Object.VehicleReservations.ToList().Count);
            //also let's test that it contains the reservation by model and id
            Assert.Contains(reservation, dbContextMock.Object.VehicleReservations.ToList());
            //also let's test that the in stock property is saved accordingly
            Assert.False(dbContextMock.Object.Vehicles.Find(1).InStock);
        }


    }

}
