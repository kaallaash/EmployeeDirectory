namespace ED.Services.Employees.Contract.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int DepartmentId { get; set; }
        public string Phone { get; set; }
        public string PhotoLink { get; set; }
    }
}
