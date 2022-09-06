using ED.Services.Employees.Contract.Models;
using ED.Services.Employees.Contract.Models.Commands;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ED.Services.Employees.Contract
{
    public interface IEmployeeService
    {
        Task<Employee> GetById(int id);

        Task<List<Employee>> GetEmployees(
            string search,
            string departmen);

        Task<int> Create(Employee employee);

        Task<int> Update(Employee employee);

        Task<int> Delete(int id);

        Task<bool> IsExistPhone(string phone);

        Task<int> CreateRandomEmployee(
            int gender, string photoLink);
    }
}
