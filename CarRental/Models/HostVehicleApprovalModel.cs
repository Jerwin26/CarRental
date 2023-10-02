using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class HostVehicleApprovalModel
    {
        
        public int VehicleID { get; set; }
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Brand { get; set; }
        public byte[] VehicleImage { get; set; }
        public string FuelType { get; set; }
        public string LicensePlate { get; set; }
        public string HostingVehicleApprovalStatus { get; set; }
        public decimal VehiclePrice { get; set; }
        public string VehicleStatus { get; set; }
       
    }
}