using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialErp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Department_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Department_Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Department_ID);
                    table.CheckConstraint("CK_Departments_Name", "LEN(LTRIM(RTRIM([Department_Name]))) > 0");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Employee_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department_ID = table.Column<int>(type: "int", nullable: false),
                    Employee_First_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Employee_Last_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Date_of_Birth = table.Column<DateOnly>(type: "date", nullable: false),
                    Date_Joined = table.Column<DateOnly>(type: "date", nullable: false),
                    Employee_Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Employee_ID);
                    table.CheckConstraint("CK_Employees_Dates", "[Date_of_Birth] > '0001-01-01' AND [Date_Joined] > [Date_of_Birth] AND [Date_Joined] <= CONVERT(date, GETDATE())");
                    table.CheckConstraint("CK_Employees_FirstName", "LEN(LTRIM(RTRIM([Employee_First_name]))) > 0");
                    table.CheckConstraint("CK_Employees_Gender", "[Gender] IN ('Male', 'Female', 'Other', 'Prefer not to say')");
                    table.CheckConstraint("CK_Employees_LastName", "LEN(LTRIM(RTRIM([Employee_Last_Name]))) > 0");
                    table.ForeignKey(
                        name: "FK_Employees_Departments_Department_ID",
                        column: x => x.Department_ID,
                        principalTable: "Departments",
                        principalColumn: "Department_ID");
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Department_ID", "Department_Address", "Department_Name" },
                values: new object[,]
                {
                    { 1, "Head Office", "Information Technology" },
                    { 2, "Head Office", "Human Resources" },
                    { 3, "Head Office", "Accounting & Finance" },
                    { 4, "Factory Building A", "Production" },
                    { 5, "Factory Building B", "Quality Assurance" },
                    { 6, "Warehouse Zone A", "Warehouse" },
                    { 7, "Head Office", "Procurement" },
                    { 8, "Head Office", "Sales & Export" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Employee_ID", "Date_Joined", "Date_of_Birth", "Department_ID", "Employee_Address", "Employee_First_name", "Gender", "Employee_Last_Name", "Photo" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 1, 15), new DateOnly(1990, 1, 10), 1, "Demo address 1, Bangkok", "Somchai", "Male", "Jaidee", null },
                    { 2, new DateOnly(2024, 2, 15), new DateOnly(1991, 2, 10), 4, "Demo address 2, Bangkok", "Anan", "Male", "Chaisuk", null },
                    { 3, new DateOnly(2024, 3, 15), new DateOnly(1992, 3, 10), 2, "Demo address 3, Bangkok", "Nattaya", "Female", "Deeprasert", null },
                    { 4, new DateOnly(2024, 4, 15), new DateOnly(1993, 4, 10), 3, "Demo address 4, Bangkok", "Pimchanok", "Female", "Kanjana", null },
                    { 5, new DateOnly(2024, 5, 15), new DateOnly(1994, 5, 10), 6, "Demo address 5, Bangkok", "Kittipong", "Male", "Arun", null },
                    { 6, new DateOnly(2024, 6, 15), new DateOnly(1995, 6, 10), 5, "Demo address 6, Bangkok", "Sudarat", "Female", "Meechai", null },
                    { 7, new DateOnly(2024, 7, 15), new DateOnly(1996, 7, 10), 7, "Demo address 7, Bangkok", "Thanawat", "Male", "Wongsa", null },
                    { 8, new DateOnly(2024, 8, 15), new DateOnly(1997, 8, 10), 8, "Demo address 8, Bangkok", "Jirawat", "Male", "Intara", null },
                    { 9, new DateOnly(2024, 9, 15), new DateOnly(1998, 9, 10), 4, "Demo address 9, Bangkok", "Waranya", "Female", "Saelim", null },
                    { 10, new DateOnly(2024, 10, 15), new DateOnly(1999, 10, 10), 1, "Demo address 10, Bangkok", "Pattarapong", "Male", "Bunmee", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Department_ID",
                table: "Employees",
                column: "Department_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
