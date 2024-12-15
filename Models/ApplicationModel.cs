﻿namespace PassportGenerationSystem.Models
{
    public class ApplicationModel
    {
        // Model

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public IFormFile Photo { get; set; } // Changed to IFormFile
        public IFormFile Document { get; set; } // Changed to IFormFile
        public string GuardianName { get; set; }
        public string MaritalStatus { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
