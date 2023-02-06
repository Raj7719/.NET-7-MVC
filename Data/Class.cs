namespace SchoolManagementApp.MVC.Data;

public sealed class Class
{
    public int Id { get; set; }

    public int? LecturerId { get; set; }

    public int? CoursesId { get; set; }

    public TimeSpan? Time { get; set; }

    public Course? Courses { get; set; }

    public ICollection<Enrollment> Enrollments { get; } = new List<Enrollment>();

    public Lecturer? Lecturer { get; set; }
}
