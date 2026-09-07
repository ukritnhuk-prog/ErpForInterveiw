using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

// Entirely fictional people and internal location labels, for demonstration only.
internal static class DemoData
{
    internal static void Configure(ModelBuilder modelBuilder)
    {
        var departments = new (string Name, string Address)[]
        {
            ("Information Technology", "Head Office"),
            ("Human Resources", "Head Office"),
            ("Accounting & Finance", "Head Office"),
            ("Production", "Factory Building A"),
            ("Quality Assurance", "Factory Building B"),
            ("Warehouse", "Warehouse Zone A"),
            ("Procurement", "Head Office"),
            ("Sales & Export", "Head Office")
        };

        modelBuilder.Entity<Department>().HasData(
            departments.Select((department, index) => new Department
            {
                DepartmentId = index + 1,
                DepartmentName = department.Name,
                DepartmentAddress = department.Address
            }));

        var employees = new (int DepartmentId, string FirstName, string LastName, string Gender)[]
        {
            (1, "Somchai", "Jaidee", "Male"),
            (4, "Anan", "Chaisuk", "Male"),
            (2, "Nattaya", "Deeprasert", "Female"),
            (3, "Pimchanok", "Kanjana", "Female"),
            (6, "Kittipong", "Arun", "Male"),
            (5, "Sudarat", "Meechai", "Female"),
            (7, "Thanawat", "Wongsa", "Male"),
            (8, "Jirawat", "Intara", "Male"),
            (4, "Waranya", "Saelim", "Female"),
            (1, "Pattarapong", "Bunmee", "Male")
        };

        modelBuilder.Entity<Employee>().HasData(
            employees.Select((employee, index) => new Employee
            {
                EmployeeId = index + 1,
                DepartmentId = employee.DepartmentId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                DateOfBirth = new DateOnly(1990 + index, index + 1, 10),
                DateJoined = new DateOnly(2024, index + 1, 15),
                EmployeeAddress = $"Demo address {index + 1}, Bangkok",
                Photo = null
            }));
    }
}
