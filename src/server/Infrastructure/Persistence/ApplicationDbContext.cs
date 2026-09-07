using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureDepartment(modelBuilder.Entity<Department>());
        ConfigureEmployee(modelBuilder.Entity<Employee>());
        DemoData.Configure(modelBuilder);
    }

    private static void ConfigureDepartment(EntityTypeBuilder<Department> entity)
    {
        entity.ToTable("Departments", table =>
            table.HasCheckConstraint(
                "CK_Departments_Name",
                "LEN(LTRIM(RTRIM([Department_Name]))) > 0"));

        entity.HasKey(department => department.DepartmentId);

        entity.Property(department => department.DepartmentId)
            .HasColumnName("Department_ID");

        entity.Property(department => department.DepartmentName)
            .HasColumnName("Department_Name")
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(department => department.DepartmentAddress)
            .HasColumnName("Department_Address")
            .HasMaxLength(500);
    }

    private static void ConfigureEmployee(EntityTypeBuilder<Employee> entity)
    {
        entity.ToTable("Employees", table =>
        {
            table.HasCheckConstraint(
                "CK_Employees_FirstName",
                "LEN(LTRIM(RTRIM([Employee_First_name]))) > 0");

            table.HasCheckConstraint(
                "CK_Employees_LastName",
                "LEN(LTRIM(RTRIM([Employee_Last_Name]))) > 0");

            table.HasCheckConstraint(
                "CK_Employees_Gender",
                "[Gender] IN ('Male', 'Female', 'Other', 'Prefer not to say')");

            table.HasCheckConstraint(
                "CK_Employees_Dates",
                "[Date_of_Birth] > '0001-01-01' " +
                "AND [Date_Joined] > [Date_of_Birth] " +
                "AND [Date_Joined] <= CONVERT(date, GETDATE())");
        });

        entity.HasKey(employee => employee.EmployeeId);

        entity.Property(employee => employee.EmployeeId)
            .HasColumnName("Employee_ID");

        entity.Property(employee => employee.DepartmentId)
            .HasColumnName("Department_ID");

        entity.Property(employee => employee.FirstName)
            .HasColumnName("Employee_First_name")
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(employee => employee.LastName)
            .HasColumnName("Employee_Last_Name")
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(employee => employee.Gender)
            .HasMaxLength(20)
            .IsRequired();

        entity.Property(employee => employee.DateOfBirth)
            .HasColumnName("Date_of_Birth")
            .HasColumnType("date");

        entity.Property(employee => employee.DateJoined)
            .HasColumnName("Date_Joined")
            .HasColumnType("date");

        entity.Property(employee => employee.EmployeeAddress)
            .HasColumnName("Employee_Address")
            .HasMaxLength(500);

        entity.Property(employee => employee.Photo)
            .HasMaxLength(500);

        entity.HasOne(employee => employee.Department)
            .WithMany(department => department.Employees)
            .HasForeignKey(employee => employee.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
