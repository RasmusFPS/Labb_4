using System;
using System.Collections.Generic;

namespace Labb_4.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? RoleId { get; set; }

    public int? DepartmentId { get; set; }

    public decimal? Salary { get; set; }

    public DateOnly? HireDate { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Role? Role { get; set; }
}
