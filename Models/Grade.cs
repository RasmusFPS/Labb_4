using System;
using System.Collections.Generic;

namespace Labb_4.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int? StudentId { get; set; }

    public int? TeacherId { get; set; }

    public int? SubjectId { get; set; }

    public string? Grade1 { get; set; }

    public DateOnly? GradeDate { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Staff? Teacher { get; set; }
}
