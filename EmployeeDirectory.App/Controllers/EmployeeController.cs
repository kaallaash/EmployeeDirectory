using ED.Services.Employees.Contract;
using ED.Services.Employees.Contract.Models;

using EmployeeDirectory.App.Helpers;
using EmployeeDirectory.App.Models;
using EmployeeDirectory.App.ViewModels;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeDirectory.App.Controllers
{
    public class EmployeeController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private IEmployeeService _employeeService;
        private IDepartmentService _departmentService;
        private const string AllDepartmentsName = "Все отделы";

        public EmployeeController(
            IWebHostEnvironment appEnvironment,
            IEmployeeService employeeService,
            IDepartmentService departmentService)
        {            
            _employeeService = employeeService;
            _appEnvironment = appEnvironment;
            _departmentService = departmentService;
        }

        public async Task<ActionResult> Index(
            int page = 1,
            string search = default,
            string department = AllDepartmentsName)
        {
            var employees = await _employeeService.GetEmployees(search, department);
            var pageSize = 5;
            var count = employees.Count();
            employees = employees
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var employeeViewModelList = new List<EmployeeViewModel>();

            foreach (var employee in employees)
            {
                employeeViewModelList.Add(await MapToEmployeeViewModel(employee));
            }

            var indexViewModel = new IndexViewModel<EmployeeViewModel>()
            {
                Items = employeeViewModelList,
                PageModel = new PageModel(count, page, pageSize),
                Filters = await GetDepartmentsFilter(department),
                FilterName = department,
                SearchPhrase = search
            };

            return View(indexViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var departments = await _departmentService.GetNames();
            ViewBag.Departments = new SelectList(departments);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(
            CreateEmployeeViewModel employeeViewModel)
        {

            if (await _employeeService.IsExistPhone(employeeViewModel.Phone))
            {
                ViewBag.Message = $"Сотрудник с телефоном {employeeViewModel.Phone}" +
                    $" уже зарегистрирован...";
                return await Create();
            }

            var employee = new Employee()
            {
                FirstName = employeeViewModel.FirstName,
                LastName = employeeViewModel.LastName,
                Patronymic = employeeViewModel.Patronymic,
                DepartmentId = await _departmentService.GetId(
                    employeeViewModel.DepartmentName),
                Phone = employeeViewModel.Phone
            };

            if (employeeViewModel.Photo != null)
            {
                var path = EmployeeHelper.GenerateEmployeeRowPhotoLink(
                    employeeViewModel.Photo.FileName);

                using (var fileStream = 
                    new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await employeeViewModel.Photo.CopyToAsync(fileStream);
                }

                employee.PhotoLink = path;
            }
            else
            {
                employee.PhotoLink = "/EmployeePhoto/_defaultUser.jpg";
            }

            var id = await _employeeService.Create(employee);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("This request isn't valid.");
            }

            var employee = await _employeeService.GetById((int)id);

            if (employee is null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Departments = await GetDepartments();

            if (employee.PhotoLink == null)
            {
                employee.PhotoLink = "/EmployeePhoto/_defaultUser.jpg";
            }

            return View(await MapToCreateEmployeeViewModel(employee));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(
            CreateEmployeeViewModel updatedEmployee)
        {
            var oldVersionEmployee = await _employeeService.GetById(updatedEmployee.Id);

            var newEmployee = new Employee()
            {
                Id = updatedEmployee.Id,
                FirstName = updatedEmployee.FirstName,
                LastName = updatedEmployee.LastName,
                Patronymic = updatedEmployee.Patronymic,
                DepartmentId = await _departmentService.GetId(
                    updatedEmployee.DepartmentName),
                Phone = updatedEmployee.Phone,
                PhotoLink = oldVersionEmployee.PhotoLink
            };

            if (updatedEmployee.Photo != null)
            {
                var path = EmployeeHelper.GenerateEmployeeRowPhotoLink(
                    updatedEmployee.Photo.FileName);

                using (var fileStream =
                    new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await updatedEmployee.Photo.CopyToAsync(fileStream);
                }

                newEmployee.PhotoLink = path;

                await FileHelper.Delete(
                    _appEnvironment.WebRootPath + oldVersionEmployee.PhotoLink);
            }

            await _employeeService.Update(newEmployee);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> CreateRandomUser()
        {
            var rnd = new Random();
            var gender = rnd.Next(0, 2);
            var defaultPhotoLink = gender == 0 ? 
                $"/EmployeePhoto/RandomUser/Male/{rnd.Next(1, 26)}.jpg" 
                : $"/EmployeePhoto/RandomUser/FeMale/{rnd.Next(1, 13)}.jpg";

            var photoLink =
                EmployeeHelper.GenerateEmployeeRowPhotoLink(defaultPhotoLink);

            var fileInfo = new FileInfo(_appEnvironment.WebRootPath + defaultPhotoLink);

            if (fileInfo.Exists)
            {
                fileInfo.CopyTo(_appEnvironment.WebRootPath + photoLink, true);
            }

            await _employeeService.CreateRandomEmployee(gender, photoLink);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(
            int id,
            int page = 1,
            string search = default,
            string department = AllDepartmentsName)
        {
            var employee = await _employeeService.GetById(id);

            await _employeeService.Delete(id);

            await FileHelper.Delete(
                _appEnvironment.WebRootPath + employee.PhotoLink);

            return  RedirectToAction("Index", new { page, search, department });
        }

        private async Task<EmployeeViewModel> MapToEmployeeViewModel(Employee employee)
        {
            return new EmployeeViewModel(
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.Patronymic,
                await _departmentService.GetName(employee.DepartmentId),
                employee.Phone,
                employee.PhotoLink);
        }

        private async Task<CreateEmployeeViewModel> MapToCreateEmployeeViewModel(
            Employee employee)
        {
            return new CreateEmployeeViewModel(
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.Patronymic,
                await _departmentService.GetName(employee.DepartmentId),
                employee.Phone,
                employee.PhotoLink,
                null);
        }

        private async Task<SelectList> GetDepartments()
        {
            return new SelectList(await _departmentService.GetNames());
        }

        private async Task<SelectList> GetDepartmentsFilter(
            string excludedName = "")
        {
            var departments = await _departmentService.GetNames();
            departments.Insert(0, AllDepartmentsName);
            departments.Remove(excludedName);

            return new SelectList(departments);
        }
    }
}
