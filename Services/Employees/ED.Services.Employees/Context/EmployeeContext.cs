using ED.Services.Employees.Context.Entities;

using Microsoft.EntityFrameworkCore;
using System;

namespace ED.Services.Employees.Context
{
    public class EmployeeContext : DbContext
    {
        public DbSet<DepartmentRow> Departments { get; set; }
        public DbSet<EmployeeRow> Employees { get; set; }
      
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentRow>()
                .HasIndex(d => d.Name).IsUnique();

            modelBuilder.Entity<EmployeeRow>()
                .HasIndex(e => e.Phone).IsUnique();

            modelBuilder.Entity<DepartmentRow>().HasData(
                new DepartmentRow
                {
                    Id = 1,
                    Name = "Разработка ПО",
                    DateCreated = DateTimeOffset.Now,
                    DateUpdated = DateTimeOffset.Now
                });

            modelBuilder.Entity<EmployeeRow>().HasData(
                new EmployeeRow
                {
                    Id = 1,
                    FirstName = "Андрей",
                    LastName = "Калашников",
                    Patronymic = "Андреевич",
                    Phone = "+79670490712",
                    DepartmentId = 1,
                    PhotoLink = "/EmployeePhoto/_dev0.jpg",
                    DateCreated = DateTimeOffset.Now,
                    DateUpdated = DateTimeOffset.Now
                });
        }
    }
}
