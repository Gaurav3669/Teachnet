using System;
using System.Collections.Generic;

namespace SampleProject.Models;

public partial class Assessment
{
    public Guid AssessmentId { get; set; }

    public Guid CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string Questions { get; set; } = null!;

    public int MaxScore { get; set; }
}
