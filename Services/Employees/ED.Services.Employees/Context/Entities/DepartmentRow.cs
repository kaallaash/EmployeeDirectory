using System;
using System.Collections.Generic;

namespace ED.Services.Employees.Context.Entities
{
    public class DepartmentRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public List<EmployeeRow> Employees { get; set; }
    }
}
