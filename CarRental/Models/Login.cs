using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class Login
    {
        public int customerId { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required.")]
        public string username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public int role { get; set; }
    }
}
