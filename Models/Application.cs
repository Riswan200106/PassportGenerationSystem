using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PassportGenerationSystem.Models
{
    public class Application
    {
        public int AppID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain letters only.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain letters only.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must start with 6, 7, 8, or 9 and be 10 digits long.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [Display(Name = "City")]
        public string City { get; set; }

        [BindNever]
        [Display(Name = "Applicant Photo")]
        public string PhotoBase64 { get; set; } = null;

        [BindNever]
        [Display(Name = "Proof Document")]
        public string DocumentBase64 { get; set; } = null;

        [Required(ErrorMessage = "Guardian Name is required.")]
        [Display(Name = "Guardian Name")]
        public string GuardianName { get; set; }

        [Required(ErrorMessage = "Marital Status is required.")]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        public string Status { get; set; }

        public int UserId { get; set; } 

    }
}
