using System.ComponentModel.DataAnnotations;

namespace PassportGenerationSystem.Models
{
    public class Feedback
    {
        public int FeedId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain letters only.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
