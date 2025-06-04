using System;
using System.ComponentModel.DataAnnotations;

namespace SampleProject.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; } // "Student" or "Instructor"

        [Required]
        public string Password { get; set; } // Plain text, to be hashed before saving
    }
}
