namespace SchoolManagementApp.MVC.Data;

public sealed partial class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public ICollection<Enrollment> Enrollments { get; } = new List<Enrollment>();
}
