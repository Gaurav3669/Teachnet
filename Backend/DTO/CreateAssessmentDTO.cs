using System;
using System.ComponentModel.DataAnnotations;

namespace SampleProject.DTOs
{
    public class CreateAssessmentDTO
    {
        [Required]
        public Guid CourseId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Questions { get; set; } // Store as JSON string

        [Required]
        public int MaxScore { get; set; }
    }
}
