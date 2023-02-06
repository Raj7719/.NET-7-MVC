namespace SchoolManagementApp.MVC.Models;

public class ClassEnrollmentViewModel
{
    public ClassViewModel? Class {get; set;}

    public List<StudentEnrollmentViewModel> Enrollment { get; set; } = new ();
}
