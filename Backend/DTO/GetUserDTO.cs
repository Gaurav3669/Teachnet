﻿using System;

namespace SampleProject.DTOs
{
    public class GetUserDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
