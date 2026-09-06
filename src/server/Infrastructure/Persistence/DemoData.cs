using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

// Entirely fictional people and internal location labels, for demonstration only.
internal static class DemoData
{
    internal static void Configure(ModelBuilder modelBuilder)
    {
        string[] names = ["Information Technology", "Human Resources", "Accounting & Finance", "Production",
            "Quality Assurance", "Warehouse", "Procurement", "Sales & Export"];
        string[] addresses = ["Head Office", "Head Office", "Head Office", "Factory Building A",
            "Factory Building B", "Warehouse Zone A", "Head Office", "Head Office"];
        modelBuilder.Entity<Department>().HasData(names.Select((name, i) => new Department
        {
            DepartmentId = i + 1, DepartmentName = name, DepartmentAddress = addresses[i]
        }));

        string[] first = ["Somchai", "Anan", "Nattaya", "Pimchanok", "Kittipong",
            "Sudarat", "Thanawat", "Jirawat", "Waranya", "Pattarapong"];
        string[] last = ["Jaidee", "Chaisuk", "Deeprasert", "Kanjana", "Arun",
            "Meechai", "Wongsa", "Intara", "Saelim", "Bunmee"];
        int[] departments = [1, 4, 2, 3, 6, 5, 7, 8, 4, 1];
        string[] genders = ["Male", "Male", "Female", "Female", "Male", "Female", "Male", "Male", "Female", "Male"];
        modelBuilder.Entity<Employee>().HasData(first.Select((name, i) => new Employee
        {
            EmployeeId = i + 1, DepartmentId = departments[i], FirstName = name, LastName = last[i],
            Gender = genders[i], DateOfBirth = new DateOnly(1990 + i, i + 1, 10),
            DateJoined = new DateOnly(2024, i + 1, 15), EmployeeAddress = $"Demo address {i + 1}, Bangkok",
            Photo = null
        }));
    }
}
