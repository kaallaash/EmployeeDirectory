using ED.Services.Employees.Context;
using ED.Services.Employees.Context.Entities;
using ED.Services.Employees.Contract;
using ED.Services.Employees.Contract.Models;
using ED.Services.Employees.Contract.Models.Commands;
using ED.Services.Employees.Helpers;
using Microsoft.EntityFrameworkCore;
using NUlid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ED.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        //IWebHostEnvironment _appEnvironment;
        private readonly EmployeeContext _dbContext;
        private const string AllDepartmentsName = "Все отделы";

        public EmployeeService(
            EmployeeContext dbContext
            //,
            //IWebHostEnvironment appEnvironment
            )
        {
            _dbContext = dbContext;
            //_appEnvironment = appEnvironment;
        }

        public async Task<Employee> GetById(int id)
        {
            return MapToEmployee(
                await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id));
        }

        public async Task<List<Employee>> GetEmployees(
            string search = default,
            string departmentName = AllDepartmentsName)
        {
           
            var EmployeeRowList = new List<EmployeeRow>();

            if (departmentName != AllDepartmentsName)
            {
                var department = 
                    await _dbContext.Departments.FirstOrDefaultAsync(d => d.Name == departmentName);

                IQueryable<EmployeeRow> employeesIQueryble = _dbContext.Employees
                    .Where(e => e.DepartmentId == department.Id)
                    .OrderBy(e => e.LastName);

                EmployeeRowList = await employeesIQueryble.ToListAsync();
            }

            if (search != null && departmentName == AllDepartmentsName)
            {
                EmployeeRowList = _dbContext.Employees
                   .ToList()
                   .Where(e => IsMatch(
                       e.LastName
                       + " " + e.FirstName
                       + " " + e.Patronymic
                       + " " + e.Phone,
                       search))
                   .OrderBy(e => e.LastName)
                   .ToList();
            }
            else if (search is null && departmentName == AllDepartmentsName)
            {
                IQueryable<EmployeeRow> employeesIQueryble = _dbContext.Employees
                    .OrderBy(e => e.LastName);
                EmployeeRowList = await employeesIQueryble.ToListAsync();
            }
            else if (search != null)
            {
                EmployeeRowList = EmployeeRowList
                 .Where(e => IsMatch(
                     e.LastName
                     + " " + e.FirstName
                     + " " + e.Patronymic
                     + " " + e.Phone,
                     search))
                 .ToList();
            }

            var employees = new List<Employee>();

            foreach (var employeeRow in EmployeeRowList)
            {
                employees.Add(MapToEmployee(employeeRow));
            }

            return employees;
        }

        public async Task<int> Create(Employee employee)
        {
            var employeeRow = new EmployeeRow()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                DepartmentId = employee.DepartmentId,
                Phone = employee.Phone,
                PhotoLink = employee.PhotoLink,
                DateCreated = DateTimeOffset.Now,
                DateUpdated = DateTimeOffset.Now
            };

            await _dbContext.Employees.AddAsync(employeeRow);
            await _dbContext.SaveChangesAsync();

            return employeeRow.Id;
        }

        public async Task<int> Update(Employee employee)
        {
            var oldVersionEmployee = await _dbContext.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (oldVersionEmployee is null)
            {
                return -1;
            }

            var employeeRow = new EmployeeRow()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                DepartmentId = employee.DepartmentId,
                Phone = employee.Phone,
                PhotoLink = employee.PhotoLink,
                DateCreated = oldVersionEmployee.DateCreated,
                DateUpdated = DateTimeOffset.Now
            };

            _dbContext.Update(employeeRow);
            await _dbContext.SaveChangesAsync();

            return employeeRow.Id;
        }

        public async Task<int> Delete(int id)
        {
            var employee = await _dbContext.Employees
                   .AsNoTracking()
                   .SingleOrDefaultAsync(
                       e => e.Id == id)
                   .ConfigureAwait(false);

            if (employee is null)
            {
                return -1;
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            return id;
        }

        public async Task<bool> IsExistPhone(string phone)
        {
            return await _dbContext.Employees.AnyAsync(e => e.Phone == phone);
        }

        public async Task<int> CreateRandomEmployee(
            int gender, string photoLink)
        {
            var employee = EmployeeHelper.GenerateRandomEmployee(gender);

            var rnd = new Random();
            string phone;

            for ( ; ; )
            {
                phone = "+79" + $"{rnd.Next(0, 1000000000)}";

                if (!_dbContext.Employees.Any(e => e.Phone == phone))
                {
                    break;
                }
            }

            var employeeRow = new EmployeeRow()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                DepartmentId = rnd.Next(0, _dbContext.Departments.Count()) + 1,
                Phone = phone,
                PhotoLink = photoLink,
                DateCreated = DateTimeOffset.Now,
                DateUpdated = DateTimeOffset.Now
            };

            await _dbContext.Employees.AddAsync(employeeRow);
            await _dbContext.SaveChangesAsync();

            return employeeRow.Id;
        }

        private static Employee MapToEmployee(
            EmployeeRow employeeRow)
        {
            return new Employee
            {
                Id = employeeRow.Id,
                FirstName = employeeRow.FirstName,
                LastName = employeeRow.LastName,
                Patronymic = employeeRow.Patronymic,
                DepartmentId = employeeRow.DepartmentId,
                Phone = employeeRow.Phone,
                PhotoLink = employeeRow.PhotoLink
            };
        }

        private static bool IsMatch(string str, string searchPhrase)
        {
            return str.ToLower().IndexOf(searchPhrase.ToLower()) != -1;
        }
    }
}
