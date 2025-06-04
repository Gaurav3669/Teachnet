using System;
using System.Collections.Generic;

namespace SampleProject.Models;

public partial class Result
{
    public Guid ResultId { get; set; }

    public Guid AssessmentId { get; set; }

    public Guid UserId { get; set; }

    public int Score { get; set; }

    public DateTime AttemptDate { get; set; }
}
