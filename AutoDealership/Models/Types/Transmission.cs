using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoDealership.Models.Types
{
    public class Transmission
    {
        public int NumberSpeeds { get; set; }
        public TransmissionType TransmissionType { get; set; }
    }

}