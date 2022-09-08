using AutoDealership.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoDealership.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public Brand Brand { get; set; }
        public string Model { get; set; }
        public Fuel FuelType { get; set; }
        public IEnumerable<string> Features { get; set; }
        public BodyStyle BodyStyle { get; set; }
        public Transmission Transmission { get; set; }
        public int Year { get; set; }

        //Mileage is stored in kilometers
        public int Mileage { get; set; }
        public Drivetrain DrivetrainType { get; set; }
        public string Color { get; set; }
        public string InteriorColor { get; set; }

        //Fuel efficiency is stored in l/100km
        public double FuelEfficiency { get; set; }
        public int Horsepower { get; set; }
        public int Torque { get; set; }
        public string Engine { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsForLease { get; set; }
        public bool IsForRent { get; set; }

        //If vehicle is for lease
        public int MonthlyPayment { get; set; }
        //If vehicle is for rent
        public int DailyPayment { get; set; }

        public VehicleStatus VehicleStatus { get; set; }
        public bool InStock { get; set; }

        public string CoverImageURL { get; set; }
        public List<string> ImagesURL { get; set; }
    }
}