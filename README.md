# ERP Demo

A simple ERP-style Employee and Department Management system built as a technical demonstration. All sample people and location labels are fictional; this is not an official company system.

## Features

- Employee and Department CRUD, including detail views and delete confirmations.
- Required department dropdown, department names in employee responses, and employee counts.
- Search by first name, last name, full name or department; optional department filter.
- Dashboard with totals, department breakdown and recently joined employees.
- Frontend and backend validation, safe errors and SQL foreign-key protection.
- Responsive Angular interface, notifications and table pagination (8 rows per page).
- SQL Server schema, EF migration and fictional seed data: 8 departments and 10 employees.
- Photo is a nullable database path/response field. Upload is intentionally not implemented.

## Technology and structure

- ASP.NET Core Web API / C# / .NET 8.
- Entity Framework Core 8 with Microsoft SQL Server.
- Angular 20, TypeScript, RxJS, ng-zorro-antd and reactive forms.
- CQRS requests and handlers are separated by use case, with MediatR and an application DbContext abstraction:

```text
Angular HttpClient
  → REST Controllers
  → Application commands / queries (MediatR)
  → Application request handlers / IApplicationDbContext
  → Infrastructure ApplicationDbContext / EF Core
  → SQL Server
```

`src/server/ERP.Demo.sln` contains Domain, Application, Infrastructure and WebAPI. Domain has no infrastructure package dependencies. Each command/query has its own handler; Infrastructure implements the DbContext abstraction declared by Application. Existing async/projection, DI and response-wrapper conventions are reused. Routes load frontend pages on demand.

## Requirements

- .NET SDK 8 or a newer SDK capable of targeting .NET 8, plus ASP.NET Core runtime 8.
- Node.js compatible with Angular 20 (for example Node 22.12+), and npm.
- SQL Server LocalDB on Windows, or an accessible SQL Server Developer/Express instance.
- Google Chrome is needed only for the included headless UI tests.

## Run locally — SQL Server LocalDB

Run commands from the repository root unless another directory is shown.

1. Ensure the LocalDB instance exists and is started:

   ```powershell
   SqlLocalDB info
   # If MSSQLLocalDB does not exist:
   SqlLocalDB create MSSQLLocalDB -s
   # If it already exists:
   SqlLocalDB start MSSQLLocalDB
   ```

2. The default connection in `src/server/WebAPI/appsettings.json` uses Windows Authentication:

   ```text
   Server=(localdb)\MSSQLLocalDB;Database=ErpDemo;Integrated Security=True;TrustServerCertificate=True
   ```

3. Restore packages/tools and create the database:

   ```powershell
   dotnet restore src/server/ERP.Demo.sln
   dotnet tool restore
   dotnet ef database update --project src/server/Infrastructure --startup-project src/server/WebAPI
   ```

4. Start the API:

   ```powershell
   dotnet run --project src/server/WebAPI --launch-profile http
   ```

5. In another terminal, start the frontend:

   ```powershell
   cd src/client
   npm.cmd ci
   npm.cmd start
   ```

`npm.cmd` avoids PowerShell execution-policy restrictions on `npm.ps1`. In other shells, `npm` can be used.

- Application: http://localhost:4200
- Swagger: http://localhost:5028/swagger
- Database health: http://localhost:5028/health

The Development profile applies pending migrations at startup. Seed rows are part of the migration, so restarting does not reinsert deleted demo records. Outside Development, automatic migration is disabled by default. There is no authentication or demo login in this local demo.

### Use a SQL Server instance instead of LocalDB

Set the connection string in the terminal used for both migration and API startup, for example:

```powershell
$env:ConnectionStrings__DefaultConnection = 'Server=.\SQLEXPRESS;Database=ErpDemo;Integrated Security=True;TrustServerCertificate=True'
dotnet ef database update --project src/server/Infrastructure --startup-project src/server/WebAPI
dotnet run --project src/server/WebAPI --launch-profile http
```

Replace the instance with your actual SQL Server. The Windows account must have permission to create/use the demo database. If using SQL authentication, supply the connection through environment configuration; do not commit credentials.

`TrustServerCertificate=True` is a local development setting. Frontend API URLs are configured in `src/client/src/environments/`; the demo uses port 5028 for both frontend build modes. CORS permits `http://localhost:4200`.

### Database migrations

EF Core migrations are the only schema initialization/update workflow. `src/server/Infrastructure/Migrations/20260906115420_InitialErp.cs` creates the tables, PK/FK, constraints, indexes and fictional seed data. Apply pending migrations with:

```powershell
dotnet ef database update --project src/server/Infrastructure --startup-project src/server/WebAPI
```

EF tracks applied migrations in `__EFMigrationsHistory`; running the command again does not duplicate the seed data. After changing entities or mappings, create a new migration before updating the database:

```powershell
dotnet ef migrations add DescribeYourChange --project src/server/Infrastructure --startup-project src/server/WebAPI
dotnet ef database update --project src/server/Infrastructure --startup-project src/server/WebAPI
```

This is a fresh ERP schema for a separate demo database. Legacy application tables/migrations were replaced in source; no migration or deletion was performed against the previous database.

## Database design

C# uses PascalCase; EF maps physical column names to the supplied diagram.

| Table | Columns |
| --- | --- |
| Departments | Department_ID (identity PK), Department_Name (required, 200), Department_Address (optional, 500) |
| Employees | Employee_ID (identity PK), Department_ID (required FK), Employee_First_name (required, 100), Employee_Last_Name (required, 100), Gender (20), Date_of_Birth (date), Date_Joined (date), Employee_Address (optional, 500), Photo (optional, 500) |

Departments 1 → N Employees. The FK uses NO ACTION, so a department with assigned employees cannot be deleted. The application checks this first and returns a readable 400 response; the FK also protects concurrent changes.

Names cannot be blank. Allowed genders are Male, Female, Other and Prefer not to say. Date of birth and joining are required, cannot be in the future, and joining must be later than birth. The API uses the server's current local date. Required lengths and appropriate checks are also enforced by SQL Server.

## REST API

| Method | Endpoint | Success |
| --- | --- | --- |
| GET | /api/departments | 200 — list including employeeCount |
| GET | /api/departments/{id} | 200 — detail |
| POST | /api/departments | 201 + Location |
| PUT | /api/departments/{id} | 200 — updated detail |
| DELETE | /api/departments/{id} | 204 |
| GET | /api/employees?search=Somchai&departmentId=1 | 200 — list including departmentName/fullName |
| GET | /api/employees/{id} | 200 — detail |
| POST | /api/employees | 201 + Location |
| PUT | /api/employees/{id} | 200 — updated detail |
| DELETE | /api/employees/{id} | 204 |

Both query parameters are optional. Unknown IDs return 404; validation/FK failures return 400; write concurrency conflicts return 409; unexpected errors return a generic 500 without stack traces or file paths.

Successful JSON responses use the existing envelope:

```json
{
  "succeeded": true,
  "message": null,
  "data": {
    "departmentId": 1,
    "departmentName": "Information Technology",
    "departmentAddress": "Head Office",
    "employeeCount": 2
  }
}
```

Department create/update body:

```json
{ "departmentName": "Demo Department", "departmentAddress": "Demo Office" }
```

Employee create/update body:

```json
{
  "departmentId": 1,
  "firstName": "Demo",
  "lastName": "Employee",
  "gender": "Other",
  "dateOfBirth": "1995-05-10",
  "dateJoined": "2024-01-15",
  "employeeAddress": "Fictional address"
}
```

Model-validation failures use ASP.NET ValidationProblemDetails (`errors`); handled application errors use `succeeded: false` and `message`. The frontend understands both formats.

## Verification

With the API running:

```powershell
node scripts/test-api.mjs
sqlcmd -S '(localdb)\MSSQLLocalDB' -E -d ErpDemo -i scripts/test-database.sql -b
```

The API script creates uniquely named temporary records and removes its own records. `scripts/test-database.sql` only tests constraints against an existing seeded database and rolls its changes back; it does not create or migrate the schema.

With the API and frontend running, and Chrome installed:

```powershell
cd src/client
npm.cmd run test:e2e
```

UI tests expect the default 10 employees / 8 departments, exercise the interview flow, mobile layout, empty search and connection recovery. They remove their own test records. Screenshots/results go to ignored `.artifacts/`.

Build:

```powershell
dotnet build src/server/ERP.Demo.sln --nologo
cd src/client
npm.cmd run build
```

Detailed progress and verified results: [PROJECT_CHECKLIST.md](PROJECT_CHECKLIST.md).

## Interview demo flow

1. Open Dashboard and show the 10 employees / 8 departments.
2. Create, view and edit a department.
3. Add an employee and select that department by name.
4. Show department name in the list; search, edit and open employee details.
5. Try deleting the occupied department and explain the validation and FK.
6. Delete the employee, then delete the now-empty department.
7. Show Swagger, database columns, constructor DI, CQRS handlers and async EF queries through `IApplicationDbContext`.

## Scope and Git

Photo upload and authentication are not implemented. Pagination is in the browser for this small demo; the API returns the matching list. No accounting, inventory or other ERP modules were added.

`origin` is configured to `https://github.com/ukritnhuk-prog/ErpForInterveiw.git`. This copied project was reinitialized on the `main` branch so the previous project's commits are not part of this repository. A recoverable copy of the previous Git metadata is stored outside the project directory at `outside the project workspace`.

Original brief: [ERP_REQUIREMENTS.md](ERP_REQUIREMENTS.md).
