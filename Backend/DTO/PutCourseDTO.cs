using System;

namespace SampleProject.DTOs
{
    public class PutCourseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid InstructorId { get; set; }
        public string MediaUrl { get; set; }
    }
}
