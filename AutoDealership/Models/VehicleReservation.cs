using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoDealership.Models
{
    public class VehicleReservation
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int VehicleId { get; set; }
        public bool IsTestDrive { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ReservedUntil { get; set; }
    }
}