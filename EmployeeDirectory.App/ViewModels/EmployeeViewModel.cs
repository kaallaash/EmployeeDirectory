namespace EmployeeDirectory.App.ViewModels
{
    public record EmployeeViewModel(
        int Id,
        string FirstName,
        string LastName,
        string Patronymic,
        string DepartmentName,
        string Phone,
        string PhotoLink);
}
