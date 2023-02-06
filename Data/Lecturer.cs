namespace SchoolManagementApp.MVC.Data;

public sealed class Lecturer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public ICollection<Class> Classes { get; } = new List<Class>();
}
