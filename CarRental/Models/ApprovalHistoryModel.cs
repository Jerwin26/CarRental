using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class ApprovalHistoryModel
    {
        public int RideId { get; set; }
        public int vehicleID_FK { get; set; }
        public int customerId_FK { get; set; }
        public decimal Fare { get; set; }
        public DateTime PickDate { get; set; }
        public DateTime DropDate { get; set; }
        public string PaymentMethod { get; set; }
        public string ApprovalStatus { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleLicensePlate { get; set; }

        public byte[] VehicleImage { get; set; } 

    }
}