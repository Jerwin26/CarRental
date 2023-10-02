using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
 
    public class Vehicle
    {
        public int customerId { get; set; }
        public int vehicleID { get; set; }

        [Display(Name = "Brand")]
        public string brand { get; set; }

        [Display(Name = "License plate")]

        public string licensePlate { get; set; }
        [Display(Name = "Fuel type")]

        public string fuelType { get; set; }

        [Display(Name = "Vehicle type")]

        public string vehicleType { get; set; }

        [Display(Name = "Vehicle price")]

        public decimal vehiclePrice { get; set; }

        [Display(Name = "Vehicle image")]

        public byte[] vehicleImage { get; set; }

        [Display(Name = "Vehicle status")]
        public string vehicleStatus { get; set; }

        public string Hostapprovedstatus { get; set; }


    }
}

