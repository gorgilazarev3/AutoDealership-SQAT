using AutoDealership.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealership.Tests
{
    public class TestsHelpers
    {
        public static List<Brand> GetMockBrandsList()
        {
            return new List<Brand> {
            new Brand
            {
                Id = 1,
                Name = "Mercedes",
                Vehicles = new List<Vehicle>
                {
                    GetMockVehicleList()[0]
                }

            },
                        new Brand
            {
                Id = 2,
                Name = "BMW",
                                Vehicles = new List<Vehicle>
                {
                    GetMockVehicleList()[1]
                }
            },
            };
        }

        public static List<Vehicle> GetMockVehicleList()
        {
            return new List<Vehicle>()
    {
        new Vehicle
        {
            Id = 1,
            BrandId = 1,
            Model = "E63s",
            FuelType = Fuel.GAS,
            Features = "Test",
            BodyStyle = Models.Types.BodyStyle.SEDAN,
            Price = 100000,
            Color = "Gray",
            Horsepower = 612,
            Year = 2018
        },
                new Vehicle
        {
            Id = 2,
            BrandId = 2,
            Model = "M5 Competition",
            FuelType = Fuel.GAS,
            Features = "Test",
            BodyStyle = Models.Types.BodyStyle.SEDAN,
            Price = 120000,
            Color = "Black",
            Horsepower = 625,
            Year = 2020

        },

    };
        }

        public static List<ApplicationUser> GetMockUserList()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Email = "test@user.com",
                    Id = "1",
                    FullName = "Test User",
                    UserName = "Test",
                    ViewedVehicles = new List<Vehicle>
                    {
                        new Vehicle
                        {
                                        Id = 2,
            BrandId = 2,
            Model = "M5 Competition",
            FuelType = Fuel.GAS,
            Features = "Test",
            BodyStyle = Models.Types.BodyStyle.SEDAN,
            Price = 120000,
            Color = "Black",
            Horsepower = 625,
            Year = 2020
                        }
                    }
                }
            };
        }

        public static List<VehicleReservation> GetMockReservationsList()
        {
            return new List<VehicleReservation>()
            {
                new VehicleReservation
                {
                    Id = 1,
                    IsTestDrive = false,
                    ReservedUntil = DateTime.Now,
                    UserId = "Test",
                    VehicleId = 2
                }
            };
        }
    }
}
