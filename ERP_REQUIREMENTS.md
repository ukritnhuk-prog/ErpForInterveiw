You are a senior ASP.NET Core / C# developer.

I already have an existing project that was previously built for another company.
I want to reuse that project as the base for a technical test / demo project for an ERP Developer position at the target company.

The goal is NOT to rebuild the whole project from scratch.

Your job is to:
1. Inspect the existing project first.
2. Understand its current architecture and implementation.
3. Reuse as much existing code, UI, structure, components, utilities, and CRUD patterns as reasonably possible.
4. Remove or replace all company-specific references from the previous project.
5. Convert the project into a small ERP-style Employee and Department Management System for the target company.
6. Keep the implementation simple, clean, maintainable, and suitable for a technical interview/demo.
7. Avoid over-engineering.

==================================================
PROJECT GOAL
==================================================

Create a small internal ERP module called:

ERP Demo

or

ERP Demo Employee Management System

The system should manage:

1. Departments
2. Employees

The main requirement is CRUD functionality using:

- ASP.NET Core
- C#
- RESTful Web API
- Microsoft SQL Server
- Entity Framework Core, if the existing project already uses EF Core
- Existing frontend framework from the current project

The frontend should be able to perform:

- Insert
- Update
- Delete
- View
- Search where reasonable

The system should look like an internal business/ERP application.

Do NOT build unnecessary modules such as:
- Accounting
- Purchase Order
- Inventory
- Sales
- Production Planning
unless such modules already exist and are trivial to remove/reuse.

For this technical test, focus mainly on Employee and Department Management.

==================================================
IMPORTANT: FIRST STEP
==================================================

Before changing any code, inspect the entire existing project.

Identify:

1. .NET version
2. ASP.NET project type
   - ASP.NET Core MVC
   - Web API
   - Razor Pages
   - Blazor
   - Angular + API
   - React + API
   - Vue + API
   - other
3. Existing architecture
   - Layered architecture
   - Clean Architecture
   - Repository Pattern
   - Service Layer
   - CQRS
   - Simple MVC
   - other
4. Database access
   - EF Core
   - Dapper
   - ADO.NET
   - Stored Procedure
   - raw SQL
5. Existing CRUD implementation
6. Existing reusable UI components
7. Existing layout/menu/sidebar/header
8. Existing authentication/authorization
9. Existing database configuration
10. Existing DTO/ViewModel structure
11. Existing dependency injection
12. Existing logging/error handling
13. Existing validation
14. Existing file upload mechanism
15. Existing naming conventions
16. Existing JavaScript/frontend libraries
17. Existing API calling conventions
18. Existing shared utilities

Before implementing, provide me a short plan:

- Files that can be reused
- Files that need to be modified
- Files that need to be renamed
- Files that should be deleted
- Company-specific code that must be removed
- Main implementation steps

Do NOT immediately rewrite the project.

==================================================
DATABASE DESIGN
==================================================

Create or adapt the database to support these tables.

--------------------------------------------------
TABLE: Departments
--------------------------------------------------

Fields:

Department_ID
- Primary Key
- Integer or existing key type
- Identity if appropriate

Department_Name
- Required
- String
- Reasonable max length, for example 100-200 characters

Department_Address
- Optional
- String
- Reasonable max length, for example 255-500 characters

Relationship:

One Department can have many Employees.

--------------------------------------------------
TABLE: Employees
--------------------------------------------------

Fields:

Employee_ID
- Primary Key
- Integer or existing key type
- Identity if appropriate

Department_ID
- Foreign Key
- References Departments.Department_ID
- Required unless there is a strong reason to allow null

Employee_First_Name
- Required
- String

Employee_Last_Name
- Required
- String

Gender
- String or enum
- Reasonable validation

Date_of_Birth
- Date

Date_Joined
- Date

Employee_Address
- Optional string

Photo
- Optional
- Store image path/file name instead of binary if the current project structure supports file upload
- If file upload would unnecessarily complicate the system, keep the field optional and document it

Relationship:

Departments
1 ---- N
Employees

Employees.Department_ID
references
Departments.Department_ID

==================================================
ENTITY RELATIONSHIP
==================================================

Create proper navigation properties if EF Core is used.

Example concept:

Department
- Department_ID
- Department_Name
- Department_Address
- ICollection<Employee> Employees

Employee
- Employee_ID
- Department_ID
- Employee_First_Name
- Employee_Last_Name
- Gender
- Date_of_Birth
- Date_Joined
- Employee_Address
- Photo
- Department

Use the existing naming convention if the project already has one.

==================================================
DATABASE CONSTRAINTS
==================================================

Add reasonable constraints.

Department:
- Department_Name cannot be empty

Employee:
- First Name cannot be empty
- Last Name cannot be empty
- Department must exist
- Date of Birth should not be in the future
- Date Joined should not be in the future unless existing business rules allow it
- Date Joined should normally be later than Date of Birth

Avoid excessive validation rules that are not part of the technical test.

==================================================
DATABASE CREATION
==================================================

If the current project uses EF Core:

- Add or update DbContext
- Add DbSet<Employee>
- Add DbSet<Department>
- Configure FK relationship
- Configure field lengths
- Configure required fields
- Generate migration

If the current project does NOT use EF Core:

Reuse its existing database approach.

Do not force EF Core into the project if doing so would require major restructuring.

Also create a SQL script if practical:

- Create Departments
- Create Employees
- PK
- FK
- Seed data

==================================================
MODULE 1: DEPARTMENT MANAGEMENT
==================================================

Create a Department Management module.

Required features:

1. Department List
2. Add Department
3. Edit Department
4. Department Detail
5. Delete Department

Department list should display:

- Department ID
- Department Name
- Department Address
- Employee Count if reasonably easy to implement
- Actions

Actions:
- View
- Edit
- Delete

Add Department form:

Fields:
- Department Name
- Department Address

Edit Department:
- Load existing data
- Allow update
- Validate required values

Delete Department:
- Ask for confirmation
- Handle departments that still contain employees

Preferred behavior:
Do not delete a department if employees are still assigned to it.

Return a clear error such as:

"Cannot delete this department because employees are currently assigned to it."

==================================================
MODULE 2: EMPLOYEE MANAGEMENT
==================================================

Create Employee Management.

Required features:

1. Employee List
2. Add Employee
3. Edit Employee
4. Employee Detail
5. Delete Employee

Employee list should display:

- Employee ID
- Employee Name
- Department Name
- Gender
- Date of Birth
- Date Joined
- Address where appropriate
- Photo thumbnail if implemented
- Actions

Do NOT show Department_ID as the main department information.

Join or map Department_Name and display it instead.

==================================================
ADD EMPLOYEE
==================================================

Fields:

- First Name
- Last Name
- Department
- Gender
- Date of Birth
- Date Joined
- Address
- Photo

Department must be a dropdown.

The dropdown should display:

Department_Name

but send/store:

Department_ID

Example:

<select>
IT
Human Resources
Accounting & Finance
Production
Quality Assurance
Warehouse
Procurement
Sales & Export
</select>

==================================================
EDIT EMPLOYEE
==================================================

Allow updating all reasonable fields:

- First Name
- Last Name
- Department
- Gender
- DOB
- Date Joined
- Address
- Photo

Load current department in dropdown.

==================================================
EMPLOYEE DETAIL
==================================================

Show:

Employee ID
Full Name
Department Name
Gender
Date of Birth
Date Joined
Address
Photo

Use a clean card/detail layout.

==================================================
DELETE EMPLOYEE
==================================================

Show confirmation before deletion.

Return appropriate success/error notification.

==================================================
REST API
==================================================

If the existing project uses REST API, create or adapt endpoints.

--------------------------------------------------
Department
--------------------------------------------------

GET /api/departments

Returns department list.

GET /api/departments/{id}

Returns department detail.

POST /api/departments

Creates department.

PUT /api/departments/{id}

Updates department.

DELETE /api/departments/{id}

Deletes department.

--------------------------------------------------
Employee
--------------------------------------------------

GET /api/employees

Returns employee list.

The response should preferably include DepartmentName.

GET /api/employees/{id}

Returns employee detail.

POST /api/employees

Creates employee.

PUT /api/employees/{id}

Updates employee.

DELETE /api/employees/{id}

Deletes employee.

==================================================
DTO
==================================================

If DTOs fit the existing architecture, create DTOs such as:

DepartmentDto
CreateDepartmentRequest
UpdateDepartmentRequest

EmployeeDto
CreateEmployeeRequest
UpdateEmployeeRequest

Example EmployeeDto:

{
    "employeeId": 1,
    "firstName": "Somchai",
    "lastName": "Jaidee",
    "fullName": "Somchai Jaidee",
    "departmentId": 1,
    "departmentName": "Information Technology",
    "gender": "Male",
    "dateOfBirth": "1998-05-10",
    "dateJoined": "2024-01-15",
    "employeeAddress": "Bangkok",
    "photo": null
}

Do not expose EF entities directly if the project already follows DTO separation.

However, if the existing system is intentionally simple, do not introduce unnecessary mapping complexity.

==================================================
HTTP STATUS CODES
==================================================

Use proper status codes.

GET success:
200 OK

GET not found:
404 Not Found

POST success:
201 Created

PUT success:
200 OK or 204 No Content

DELETE success:
200 OK or 204 No Content

Validation error:
400 Bad Request

Unexpected server error:
500 Internal Server Error

==================================================
VALIDATION
==================================================

Validate both frontend and backend where practical.

Examples:

Department Name:
Required

Employee First Name:
Required

Employee Last Name:
Required

Department:
Required

Date of Birth:
Must not be in future

Date Joined:
Must not be earlier than date of birth

Return readable validation messages.

==================================================
SEARCH
==================================================

If reasonably simple, add search to Employee List.

Allow search by:

- Employee First Name
- Employee Last Name
- Department Name

Example:

GET /api/employees?search=somchai

Optional filtering:

GET /api/employees?departmentId=1

Do not add complex search infrastructure.

==================================================
PAGINATION
==================================================

If the existing project already supports pagination, reuse it.

Otherwise, pagination is optional.

Do not over-engineer pagination unless easy to implement.

==================================================
DASHBOARD
==================================================

Create a simple dashboard page.

Display:

- Total Employees
- Total Departments

Optional:
- Recently Joined Employees
- Employee count grouped by Department

Example cards:

Total Employees
10

Total Departments
5

Departments breakdown:
IT - 2
Production - 3
Warehouse - 2
HR - 1
QA - 2

Keep it visually simple.

==================================================
UI / UX
==================================================

Reuse the current project UI layout.

The application should look like an internal ERP system.

Suggested navigation:

Dashboard

Employee Management
- Employees
- Add Employee

Department Management
- Departments
- Add Department

Use clean business UI.

Do not redesign everything unless necessary.

==================================================
BRANDING
==================================================

Remove all branding from the previous company.

Search across the entire codebase for:

- Previous company name
- Previous company abbreviation
- Previous system name
- Previous logo
- Previous database name
- Previous email domain
- Previous copyright
- Previous footer text
- Previous API URLs
- Previous internal URLs
- Previous server names
- Previous file paths
- Previous environment settings
- Previous organization identifiers

Replace with generic or generic ERP demo names where appropriate.

Suggested application title:

ERP Demo

Suggested subtitle:

Employee & Department Management

Footer example:

ERP Demo

Do NOT use real company confidential information.

This is only a technical demo.

==================================================
SECURITY / CONFIDENTIALITY
==================================================

The old project came from work done for another company.

Therefore:

Do not expose proprietary company information.

Remove:

- Real employee data
- Real customer data
- Credentials
- Secrets
- API keys
- Production URLs
- Production connection strings
- Internal server names
- Company internal logic
- Confidential business logic
- Company documents
- Company images/logos unless publicly available and appropriate

All test data must be fake.

If any suspicious company-specific data is found:
STOP and report it before reusing it.

==================================================
SAMPLE DEPARTMENTS
==================================================

Create at least these sample departments:

1. Information Technology
2. Human Resources
3. Accounting & Finance
4. Production
5. Quality Assurance
6. Warehouse
7. Procurement
8. Sales & Export

Department addresses can be fake/internal labels such as:

- Head Office
- Factory Building A
- Factory Building B
- Warehouse Zone A

Do not claim these are real company locations.

==================================================
SAMPLE EMPLOYEES
==================================================

Create at least 10 fake employees.

Example:

Somchai Jaidee
Information Technology

Anan Chaisuk
Production

Nattaya Deeprasert
Human Resources

Pimchanok Kanjana
Accounting & Finance

Kittipong Arun
Warehouse

Sudarat Meechai
Quality Assurance

Thanawat Wongsa
Procurement

Jirawat Intara
Sales & Export

Waranya Saelim
Production

Pattarapong Bunmee
Information Technology

Use entirely fictional data.

==================================================
PHOTO UPLOAD
==================================================

If photo upload already exists:

Reuse the current implementation.

Suggested approach:

Store file under:

wwwroot/uploads/employees/

Database stores only:

/uploads/employees/{filename}

Validate:

- jpg
- jpeg
- png

Maximum file size:
for example 2 MB

Generate safe unique filename.

Do not trust original filename.

If file upload is difficult because of current architecture:
Photo functionality can remain optional.

==================================================
ASYNC / AWAIT
==================================================

Use async database operations.

Examples:

ToListAsync
FirstOrDefaultAsync
FindAsync
SaveChangesAsync

Do not use Task.Run around database calls.

==================================================
QUERY PERFORMANCE
==================================================

For read-only queries use:

AsNoTracking()

where appropriate.

Avoid N+1 query problems.

For employee listing:

Join/include Department efficiently.

Example concept:

Employees
.Include(x => x.Department)
.AsNoTracking()

or projection:

.Select(x => new EmployeeDto
{
    EmployeeId = x.EmployeeId,
    FirstName = x.FirstName,
    LastName = x.LastName,
    DepartmentName = x.Department.DepartmentName
})

Prefer projection if consistent with current architecture.

==================================================
DEPENDENCY INJECTION
==================================================

Reuse existing DI structure.

If service interfaces already exist:

IEmployeeService
IDepartmentService

Implement:

EmployeeService
DepartmentService

If repository pattern exists:

IEmployeeRepository
EmployeeRepository

IDepartmentRepository
DepartmentRepository

Do not introduce repository pattern if the existing system uses DbContext directly and is already clean/simple.

==================================================
ERROR HANDLING
==================================================

Reuse existing global exception handling if available.

Otherwise use simple safe handling.

Do not expose stack trace to frontend.

Return readable errors.

Example:

{
    "success": false,
    "message": "Employee not found"
}

Do not create a complex custom framework solely for this project.

==================================================
LOGGING
==================================================

Reuse ILogger if available.

Log useful operations:

- Employee created
- Employee updated
- Employee deleted
- Department created
- Department updated
- Department deleted
- Unexpected failures

Never log sensitive data.

==================================================
FRONTEND REQUIREMENTS
==================================================

IMPORTANT:

Use the frontend framework that already exists in the project.

Do not migrate technologies unnecessarily.

If current project is:

Angular:
Reuse Angular.

React:
Reuse React.

Vue:
Reuse Vue.

Razor:
Reuse Razor.

MVC:
Reuse MVC views.

Do not convert frameworks just because another framework might look newer.

==================================================
FRONTEND EMPLOYEE LIST
==================================================

Build a table.

Columns:

Employee ID
Name
Department
Gender
Date Joined
Action

Action buttons:

View
Edit
Delete

Include search bar if simple.

==================================================
FRONTEND DEPARTMENT LIST
==================================================

Columns:

Department ID
Department Name
Department Address
Employee Count
Actions

==================================================
CONFIRMATION
==================================================

Before delete:

Show a confirmation dialog.

Example:

Are you sure you want to delete this employee?

For departments:

Are you sure you want to delete this department?

If employees are assigned:

Show:

Cannot delete this department because it still has assigned employees.

==================================================
NOTIFICATIONS
==================================================

Use current project's existing notification system.

Examples:

Employee created successfully.

Employee updated successfully.

Employee deleted successfully.

Department created successfully.

Department updated successfully.

Department deleted successfully.

==================================================
CODE STYLE
==================================================

Follow existing project's:

- naming
- formatting
- folder structure
- architecture
- DI
- API response format
- exception handling
- frontend style

Do not randomly mix conventions.

==================================================
DO NOT OVER-ENGINEER
==================================================

Do NOT automatically add:

- Kafka
- RabbitMQ
- Redis
- Docker
- Kubernetes
- MediatR
- CQRS
- Microservices
- Domain events
- Event sourcing
- Message queues
- Distributed cache
- ElasticSearch

unless the existing project genuinely depends on them.

This technical test is primarily CRUD + REST API + SQL Server.

Simple and complete is better than complex and unfinished.

==================================================
README
==================================================

Create or update README.md.

Include:

# ERP Demo

Description:

A simple ERP-style Employee and Department Management system built as a technical demonstration.

Features:
- Employee CRUD
- Department CRUD
- Employee/Department relationship
- REST API
- SQL Server
- Employee search
- Dashboard
- Optional photo upload

Technology:
List actual technologies detected in project.

Setup:

1. Configure connection string
2. Restore packages
3. Create/update database
4. Run migration or SQL script
5. Start backend
6. Start frontend if separate
7. Open application

Also document demo login if authentication exists.

==================================================
PROJECT NAME
==================================================

Rename project/display names only when safe.

Suggested:

ERP.Demo

or

ErpDemo.EmployeeManagement

If namespace renaming would create too much unnecessary risk, keep technical namespaces but remove visible branding from the old company.

Prioritize having a stable running project over cosmetic namespace renaming.

==================================================
FINAL REVIEW
==================================================

After implementation:

Perform a complete review.

Check:

1. Build succeeds
2. No compile errors
3. Database migration works
4. Department list works
5. Department create works
6. Department update works
7. Department detail works
8. Department delete works
9. Employee list works
10. Employee create works
11. Employee update works
12. Employee detail works
13. Employee delete works
14. Employee department dropdown works
15. Department name is displayed correctly
16. Validation works
17. Invalid IDs return safe error
18. Database relationship works
19. Delete department with employees is safely handled
20. No real data remains
21. No previous company branding remains
22. No credentials or secrets remain
23. No old production endpoint remains
24. README is updated
25. App can be demonstrated easily

==================================================
OUTPUT AFTER WORK
==================================================

At the end provide me with:

1. Summary of changes
2. Files changed
3. Files added
4. Files deleted
5. Database changes
6. API endpoints
7. UI pages
8. Any remaining known issues
9. Steps to run the project
10. Suggested demo flow for interview

==================================================
DEMO FLOW
==================================================

The expected interview/demo flow should be approximately:

1. Open Dashboard
2. Show total employees and departments
3. Open Department Management
4. Create a new department
5. Edit the department
6. Open Employee Management
7. Create a new employee
8. Select Department from dropdown
9. Save
10. Show employee in list with Department Name
11. Edit employee
12. Show employee detail
13. Delete employee
14. Explain database FK
15. Explain REST API
16. Explain architecture briefly
17. Explain validation
18. Explain why existing project code was reused

==================================================
TECHNICAL INTERVIEW QUALITY
==================================================

Code should demonstrate:

- Clear structure
- Basic OOP
- REST API understanding
- SQL relationship understanding
- Entity Framework / database access understanding
- Dependency Injection understanding
- Async/await
- Validation
- Error handling
- Maintainability
- Ability to understand an existing system rather than rewrite everything

The project does not need to demonstrate overly advanced architecture.

Prioritize:

Correctness
Readability
Maintainability
Working functionality
Reasonable UI
Clear database design

==================================================
WORKING STYLE
==================================================

Work incrementally.

Do not make hundreds of changes at once.

Recommended sequence:

STEP 1
Inspect project.

STEP 2
Report current architecture.

STEP 3
Search for old company-specific references.

STEP 4
Create database models.

STEP 5
Implement Department backend.

STEP 6
Implement Employee backend.

STEP 7
Implement Department frontend.

STEP 8
Implement Employee frontend.

STEP 9
Create Dashboard.

STEP 10
Seed sample data.

STEP 11
Remove old branding/data.

STEP 12
Build and fix errors.

STEP 13
Test CRUD.

STEP 14
Update README.

STEP 15
Provide final summary.

At the end of each major step:
- Build the project
- Fix errors before moving to the next step

Do not leave broken intermediate code if avoidable.