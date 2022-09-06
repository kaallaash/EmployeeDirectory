using System;

namespace ED.Services.Employees.Context.Entities
{
    public class EmployeeRow
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int DepartmentId { get; set; }
        public string Phone { get; set; }
        public string PhotoLink { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DepartmentRow Department { get; set; }
    }
}
