using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Departments", table =>
                table.HasCheckConstraint("CK_Departments_Name", "LEN(LTRIM(RTRIM([Department_Name]))) > 0"));
            entity.HasKey(d => d.DepartmentId);
            entity.Property(d => d.DepartmentId).HasColumnName("Department_ID");
            entity.Property(d => d.DepartmentName).HasColumnName("Department_Name").HasMaxLength(200).IsRequired();
            entity.Property(d => d.DepartmentAddress).HasColumnName("Department_Address").HasMaxLength(500);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees", table =>
            {
                table.HasCheckConstraint("CK_Employees_FirstName", "LEN(LTRIM(RTRIM([Employee_First_name]))) > 0");
                table.HasCheckConstraint("CK_Employees_LastName", "LEN(LTRIM(RTRIM([Employee_Last_Name]))) > 0");
                table.HasCheckConstraint("CK_Employees_Gender", "[Gender] IN ('Male', 'Female', 'Other', 'Prefer not to say')");
                table.HasCheckConstraint("CK_Employees_Dates",
                    "[Date_of_Birth] > '0001-01-01' AND [Date_Joined] > [Date_of_Birth] AND [Date_Joined] <= CONVERT(date, GETDATE())");
            });
            entity.HasKey(e => e.EmployeeId);
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.DepartmentId).HasColumnName("Department_ID");
            entity.Property(e => e.FirstName).HasColumnName("Employee_First_name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasColumnName("Employee_Last_Name").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Gender).HasMaxLength(20).IsRequired();
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_of_Birth").HasColumnType("date");
            entity.Property(e => e.DateJoined).HasColumnName("Date_Joined").HasColumnType("date");
            entity.Property(e => e.EmployeeAddress).HasColumnName("Employee_Address").HasMaxLength(500);
            entity.Property(e => e.Photo).HasMaxLength(500);
            entity.HasOne(e => e.Department).WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId).OnDelete(DeleteBehavior.NoAction);
        });

        DemoData.Configure(modelBuilder);
    }
}
