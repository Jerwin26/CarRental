using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class BookRide
    {
        public int RideId { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public decimal Fare { get; set; }

        [DataType(DataType.Date)]
        public DateTime PickDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DropDate { get; set; }

        public string  PaymentMethod { get; set; }
        public string ApprovalStatus { get; set; }

        public string Brand { get; set; }
        public string LicensePlate { get; set; }
        public string FuelType { get; set; }
        public string VehicleType { get; set; }
        public decimal VehiclePrice { get; set; }
        public byte[] VehicleImage { get; set; }
        public string VehicleStatus { get; set; }



    }
}
