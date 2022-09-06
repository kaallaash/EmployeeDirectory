using Microsoft.AspNetCore.Http;

namespace EmployeeDirectory.App.ViewModels
{
    public record CreateEmployeeViewModel(
        int Id,
        string FirstName,
        string LastName,
        string Patronymic,
        string DepartmentName,
        string Phone,
        string PhotoLink,
        IFormFile Photo);
}