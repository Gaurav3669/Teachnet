using System;
using System.Collections.Generic;

namespace SampleProject.Models;

public partial class Course
{
    public Guid CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid InstructorId { get; set; }

    public string MediaUrl { get; set; } = null!;
}
