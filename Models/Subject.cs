using System;
using System.Collections.Generic;

namespace Labb_4.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
