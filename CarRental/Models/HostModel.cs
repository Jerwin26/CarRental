using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class HostModel
    {
        [Display(Name = "Host id")]

        public int customerId { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required.")]
        public string firstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string lastName { get; set; }

        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime dateOfBirth { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is required.")]
        public string gender { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format.")]
        public string phoneNumber { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string emailAddress { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required.")]
        public string state { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required.")]
        public string city { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required.")]
        public string username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string password { get; set; }

        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

       


    }
}