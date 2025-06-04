using System;
using System.ComponentModel.DataAnnotations;

namespace SampleProject.DTOs
{
    public class CreateCourseDTO
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid InstructorId { get; set; }

        public string MediaUrl { get; set; } // Optional: URL to course video/file in Blob
    }
}
