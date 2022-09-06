using ED.Services.Employees.Contract;
using ED.Services.Employees.Contract.Models;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace EmployeeDirectory.App.Controllers
{
    public class DepartmentController : Controller
    {
        private IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetDepartments();

            return View(departments);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Department department)
        {
            var id = await _departmentService.Create(department);

            if (id == -1)
            {
                ViewBag.Message = $"Отдел с именем \"{department.Name}\" уже существует...";
                return await Create();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest("This request isn't valid.");
            }

            var department = await _departmentService.Get((int)id);

            if (department is null)
            {
                return RedirectToAction("Index");
            }

            return View(department);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Department department)
        {
            await _departmentService.Update(department);

            return RedirectToAction("Index");
        }
    }
}
