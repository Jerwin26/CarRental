using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class Contact
    {
        [Display(Name = "Full name")]
        [Required(ErrorMessage = "Full name is required.")]
        public string fullName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string email { get; set; }

        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format.")]
        public string phoneNumber { get; set; }

        [Display(Name = "Message")]
        [Required(ErrorMessage = "Message is required.")]
        public string message { get; set; }
    }
}
