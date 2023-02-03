using System;
using System.Collections.Generic;

namespace SchoolManagementApp.MVC.Data;

public partial class Class
{
    public int Id { get; set; }

    public int? LecturerId { get; set; }

    public int? CoursesId { get; set; }

    public TimeSpan? Time { get; set; }

    public virtual Course? Courses { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; } = new List<Enrollment>();

    public virtual Lecturer? Lecturer { get; set; }
}
