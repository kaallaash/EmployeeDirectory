using ED.Services.Employees.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED.Services.Employees.Contract
{
    public interface IDepartmentService
    {
        Task<Department> Get(int id);

        Task<List<Department>> GetDepartments();

        Task<string> GetName(int id);

        Task<int> GetId(string name);

        Task<List<string>> GetNames();

        Task<int> Create(Department department);

        Task<int> Update(Department department);
    }
}
