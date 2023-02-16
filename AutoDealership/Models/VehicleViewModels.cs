using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoDealership.Models
{
    public class CreateVehicleViewModel
    {
        public Vehicle Vehicle { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public Brand NewBrand { get; set; }
        public string ListOfFeatures { get; set; }
        public string ListOfImages { get; set; }
    }

    public class VehicleDetailsViewModel
    {
        public Vehicle Vehicle { get; set; }
        public Brand Brand { get; set; }
    }

    public class MyReservationViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Vehicle> TestDriveVehicles { get; set; }
        public Vehicle ReservedVehicle { get; set; }
        public VehicleReservation ReservationDetails { get; set; }
        public List<VehicleReservation> TestDrivesDetails { get; set; }
    }

    public class InventoryViewModel
    {
        public List<Vehicle> Inventory { get; set; }
        public string[] SearchQuery { get; set; }
        public List<Brand> AllBrands { get; set; }
        public int NumCols { get; set; }
        public string SortOrder { get; set; }
    }

}