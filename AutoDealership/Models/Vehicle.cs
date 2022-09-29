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
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public string Model { get; set; }
        [Display(Name = "Fuel Type")]
        public Fuel FuelType { get; set; }
        public string Features { get; set; }
        [Display(Name = "Body Style")]
        public BodyStyle BodyStyle { get; set; }
        public Transmission Transmission { get; set; }
        public int Year { get; set; }

        //Mileage is stored in kilometers
        public int Mileage { get; set; }
        [Display(Name = "Drivetrain")]
        public Drivetrain DrivetrainType { get; set; }
        [Display(Name = "Exterior Color")]
        public string Color { get; set; }
        [Display(Name = "Interior Color")]
        public string InteriorColor { get; set; }

        //Fuel efficiency is stored in l/100km
        [Display(Name = "Fuel Efficiency")]
        public double FuelEfficiency { get; set; }
        [Display(Name = "Power")]
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
        [Display (Name = "Vehicle Status")]
        public VehicleStatus VehicleStatus { get; set; }
        public bool InStock { get; set; }
        public bool IsTestDriven { get; set; }
        [Display(Name = "Vehicle Image")]
        public string CoverImageURL { get; set; }
        public string ImagesURL { get; set; }


        public double GetMPG()
        {
            return FuelEfficiency * 235.214583;
        }  
    }
}