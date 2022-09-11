using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoDealership.Models.Types
{
    public class Transmission
    {
        [Display(Name = "Number of speeds")]
        public int NumberSpeeds { get; set; }
        [Display(Name = "Transmission Type")]
        public TransmissionType TransmissionType { get; set; }
    }

}