using ED.Services.Employees.Context;
using ED.Services.Employees.Context.Entities;
using ED.Services.Employees.Contract;
using ED.Services.Employees.Contract.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED.Services.Employees
{
    public class DepartmentService : IDepartmentService
    {
        private readonly EmployeeContext _dbContext;
        private const string AllDepartmentsName = "Все отделы";

        public DepartmentService(
            EmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Department> Get(int id)
        {
            var departmentRow =
                await _dbContext.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            return MapToDepartment(departmentRow);
        }

        public async Task<List<Department>> GetDepartments()
        {
            var departmentRows =
                await _dbContext.Departments
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .ToListAsync();

            var departments = new List<Department>();

            foreach (var departmentRow in departmentRows)
            {
                departments.Add(MapToDepartment(departmentRow));
            }

            return departments;
        }

        public async Task<int> GetId(string name)
        {
            var department =
                await _dbContext.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Name == name);

            return department.Id;
        }

        public async Task<string> GetName(int id)
        {
            var department =
                await _dbContext.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            return department.Name;
        }

        public async Task<List<string>> GetNames()
        {
            return await _dbContext.Departments
                .Select(d => d.Name)
                .ToListAsync();
        }

        public async Task<int> Create(Department department)
        {
            var names = await GetNames();

            var LowerCasedepartmentName = department.Name.ToLower();

            foreach (var name in names)
            {
                if (name.ToLower() == LowerCasedepartmentName)
                {
                    return -1;
                }
            }

            var departmentRow = new DepartmentRow()
            {
                Name = department.Name,
                DateCreated = DateTimeOffset.Now,
                DateUpdated = DateTimeOffset.Now
            };

            await _dbContext.Departments.AddAsync(departmentRow);
            await _dbContext.SaveChangesAsync();

            var id = await GetId(department.Name);

            return id;
        }

        public async Task<int> Update(Department department)
        {
            var oldVersionDepartment = await _dbContext.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == department.Id);

            if (oldVersionDepartment is null)
            {
                return -1;
            }

            var departmentRow = new DepartmentRow()
            {
                Id = department.Id,
                Name = department.Name,
                DateCreated = oldVersionDepartment.DateCreated,
                DateUpdated = DateTimeOffset.Now
            };

            _dbContext.Update(departmentRow);
            await _dbContext.SaveChangesAsync();

            return departmentRow.Id;
        }

        private Department MapToDepartment(DepartmentRow departmentRow)
        {
            return new Department()
            {
                Id = departmentRow.Id,
                Name = departmentRow.Name
            }; 
        }
    }
}
