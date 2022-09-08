using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoDealership.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name="Brand Name")]
        public string Name { get; set; }

        [Display(Name = "Brand Logo")]
        public string LogoURL { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}