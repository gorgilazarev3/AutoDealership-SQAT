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
    }

    public class VehicleDetailsViewModel
    {
        public Vehicle Vehicle { get; set; }
        public Brand Brand { get; set; }
    }

}