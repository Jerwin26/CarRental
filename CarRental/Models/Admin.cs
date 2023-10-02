using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class Admin
    {
        public string adminName { get; set; }

        public int adminid { get; set; }
        public string adminEmail { get; set; }
        public string adminPassword { get; set; }

        public string adminUserName { get; set; }

        public DateTime dateOfBirth { get; set; }
        public string phoneNumber { get; set; }

        public string gender { get; set; }
    }
}