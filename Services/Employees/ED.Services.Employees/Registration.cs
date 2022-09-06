using ED.Services.Employees.Context;

using ED.Services.Employees.Contract;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace ED.Services.Employees
{
    public static class Registration
    {
        public static IServiceCollection AddEmployees(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContextPool<EmployeeContext>(
                (s, b) =>
                    b.UseNpgsql(configuration.GetConnectionString("EmployeeDirectoryDb")));

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            return services;
        }
    }
}
