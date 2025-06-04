using System;

namespace SampleProject.DTOs
{
    public class PutUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        // Password is optional on update. If provided, it will be hashed and updated.
        public string? Password { get; set; }
    }
}
