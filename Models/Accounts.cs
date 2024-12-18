using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PassportGenerationSystem.Models
{
    public class Accounts
    {
        [Key]
        [Display(Name = "Account ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain letters only.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain letters only.")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must start with 6, 7, 8, or 9 and be 10 digits long.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Username")]
        [RegularExpression(@"^[a-zA-Z0-9]{6,}$", ErrorMessage = "Username must be at least 6 characters long and contain only letters and numbers.")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, and one symbol.")]
        public string Password { get; set; }

        [NotMapped] 
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }


        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
