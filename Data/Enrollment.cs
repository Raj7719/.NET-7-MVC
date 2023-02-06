namespace SchoolManagementApp.MVC.Data;

public sealed class Enrollment
{
    public int Id { get; set; }

    public int? StudentId { get; set; }

    public int? ClassId { get; set; }

    public TimeSpan? Time { get; set; }

    public Class? Class { get; set; }

    public Student? Student { get; set; }
}
