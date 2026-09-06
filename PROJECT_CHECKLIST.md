# ERP Demo — Checklist

อัปเดต: 2026-09-06

สถานะ: **พัฒนาและทดสอบฟังก์ชันหลักเสร็จแล้ว** ใช้ SQL Server โดยตรง และเปลี่ยน Git origin แล้ว

- [โจทย์ต้นฉบับ](ERP_REQUIREMENTS.md)
- [วิธีติดตั้งและรัน / API / Demo flow](README.md)
- [สรุปงานและรายการไฟล์เพิ่ม–แก้ไข–ลบ](IMPLEMENTATION_SUMMARY.md)

`[x]` หมายถึงทำและตรวจยืนยันแล้ว ส่วนสิ่งที่งดทำเพราะเป็น optional แยกไว้ท้ายเอกสาร ไม่ได้รวมเป็นงานที่ทำเสร็จ

## ข้อกำหนดล่าสุดที่ใช้

- [x] ใช้โปรเจกต์เดิมเป็นฐาน คง ASP.NET Core, Angular และสถาปัตยกรรมเดิมที่จำเป็น
- [x] Employee/Department ตามภาพ: ชื่อคอลัมน์ SQL, identity PK และ Department FK
- [x] ใช้ Microsoft SQL Server โดยตรง; ตั้งค่าเริ่มต้นเป็น LocalDB ที่ติดตั้งในเครื่อง
- [x] เปลี่ยน origin เป็น `https://github.com/ukritnhuk-prog/ErpForInterveiw.git`
- [x] เก็บโจทย์ แผนงาน ผลทดสอบ และ checklist เป็น Markdown

## 1. ตรวจโครงการและวางแผน reuse

- [x] สำรวจ server/client, project files, dependencies และการตั้งค่า
- [x] ตรวจ controller → MediatR request/handler → IApplicationDbContext → EF Core
- [x] ตรวจ DbContext, DTO/response wrapper, DI และ validation เดิม
- [x] ตรวจ Angular components, reactive forms, HttpClient และ ng-zorro
- [x] ตรวจ auth/upload: ไม่พบระบบ login หรือ upload ที่ต้อง reuse
- [x] ตรวจข้อมูลตัวอย่าง ชื่อระบบเดิม และ credentials ใน config/README ก่อนนำโค้ดมาใช้
- [x] รายงานข้อมูลเดิมที่ต้องตรวจสอบ แล้วแทนด้วยข้อมูลสมมติ
- [x] Build baseline ก่อนเปลี่ยนโมดูล; แก้ nullable warnings เดิมด้วยการนำ DTO ที่ไม่ใช้แล้วออก

## 2. โครงสร้างและ Database

- [x] คง Domain / Application / Infrastructure / WebAPI
- [x] แยก MediatR command/query และ handler ต่อ use case, ใช้ IApplicationDbContext และคง response envelope
- [x] ใช้ DataAnnotations และ IValidatableObject; นำ AutoMapper/FluentValidation ที่ไม่ใช้แล้วออก
- [x] Domain ไม่มี package dependencies ของ infrastructure
- [x] เพิ่ม Department และ Employee พร้อม navigation properties
- [x] DbSet ทั้งสองตาราง, required fields, max lengths, date types และ FK
- [x] Map คอลัมน์ตามภาพ: Department_ID, Employee_ID, Employee_First_name, Employee_Last_Name ฯลฯ
- [x] One-to-many และ NO ACTION ป้องกันลบ Department ที่มี Employee
- [x] SQL check constraints สำหรับชื่อว่าง, Gender และวันที่
- [x] สร้าง `InitialErp` migration และ model snapshot ใหม่
- [x] ใช้ EF Core Migration เป็นวิธีสร้าง/อัปเดต schema และ seed เพียงวิธีเดียวตามคำขอล่าสุด
- [x] สร้างและ migrate ฐานข้อมูล `ErpDemo` บน SQL Server LocalDB จริง
- [x] นำ SQL script สำหรับสร้างฐานข้อมูลที่เคยเพิ่มเป็นทางเลือกออก และปรับ README ให้ใช้ Migration
- [x] ใช้ฐานข้อมูล demo แยก; ไม่แก้หรือลบฐานข้อมูลระบบเดิม

## 3. Department Backend

- [x] GET list พร้อม EmployeeCount
- [x] GET detail
- [x] POST พร้อม HTTP 201 และ Location
- [x] PUT แก้ไขชื่อ/ที่อยู่
- [x] DELETE พร้อม HTTP 204
- [x] Required/whitespace/max length validation
- [x] Unknown IDs คืน 404
- [x] ป้องกันลบแผนกที่มีพนักงาน พร้อมข้อความอ่านเข้าใจ
- [x] DTO, command/query, handler, DbContext abstraction และ DI ครบ

## 4. Employee Backend

- [x] GET list พร้อม DepartmentName และ FullName
- [x] GET detail
- [x] POST พร้อม HTTP 201 และ Location
- [x] PUT แก้ไขชื่อ นามสกุล แผนก Gender วันที่ และที่อยู่
- [x] DELETE พร้อม HTTP 204
- [x] Department ต้องมีอยู่จริง
- [x] ชื่อ/นามสกุล required, whitespace และ max length
- [x] Gender ใช้ค่าที่อนุญาต
- [x] DOB/DateJoined required และไม่เป็นอนาคต
- [x] DateJoined ต้องหลัง DOB
- [x] Search ชื่อ นามสกุล ชื่อเต็ม และชื่อแผนก
- [x] Filter ตาม departmentId
- [x] Async EF calls, cancellation token, AsNoTracking และ projection/join
- [x] ไม่เรียก query หาแผนกแยกทีละ employee
- [x] DTO, command/query, handler, DbContext abstraction และ DI ครบ

## 5. Frontend

- [x] Angular 20 และ ng-zorro เดิม พร้อม styles/forms/API patterns ที่นำมาปรับใช้
- [x] Sidebar / header / footer ชื่อ ERP Demo
- [x] Routes สำหรับ Dashboard, Employees และ Departments
- [x] โหลดแต่ละ page ตาม route เพื่อลด initial bundle
- [x] Department table: ID, Name, Address, EmployeeCount, View/Edit/Delete
- [x] Department add/edit form และ detail modal
- [x] Employee table: ID, Name, DepartmentName, Gender, DOB, DateJoined, Actions
- [x] Employee add/edit form และ detail card
- [x] Dropdown แสดง DepartmentName และส่ง DepartmentId
- [x] Edit โหลดค่าปัจจุบันรวมถึง Department
- [x] Frontend validation และแสดง backend errors
- [x] Confirmation ก่อนลบ; Cancel ไม่ลบข้อมูล
- [x] Notifications หลัง create/update/delete และข้อผิดพลาด
- [x] Search/filter/reset, empty/loading/error states และ Retry
- [x] Pagination ฝั่ง browser หน้าละ 8 แถว
- [x] รองรับ desktop และมือถือ; ตารางเลื่อนในกรอบ ไม่ทำให้ทั้งหน้าล้น
- [x] Dashboard totals, department breakdown และ recently joined

## 6. Data, Configuration และ Cleanup

- [x] Seed 8 แผนกตามโจทย์และพนักงานสมมติ 10 คน
- [x] ที่อยู่และสถานที่เป็นข้อมูลสมมติ พร้อมระบุในแอปและเอกสาร
- [x] Photo เป็น nullable field/path; ระบุชัดเจนว่ายังไม่เปิด upload
- [x] ลบ Post/Comment code, sample WeatherForecast, seed/image URL และชื่อผู้ใช้ hard-coded เดิม
- [x] ลบ runtime log เดิมที่เคยอยู่ใน Git
- [x] เปลี่ยน package/display name และ solution เป็น ERP.Demo.sln
- [x] นำรหัสผ่าน hard-coded ออกจาก source/config/README ปัจจุบัน
- [x] ใช้ Windows Authentication; README อธิบาย override connection string ด้วย environment
- [x] ค้น source/scripts ปัจจุบัน ไม่พบชื่อระบบเดิม/รหัสผ่านเดิม/production endpoint ตามแพตเทิร์นที่ตรวจ
- [x] Error response ไม่ส่ง exception details, stack trace หรือ file path
- [x] ILogger สำหรับ CRUD ใช้ record ID; custom error log ใช้ exception type และ trace ID
- [x] Health check ตรวจการเชื่อมต่อ DbContext
- [x] เพิ่ม .gitignore สำหรับ tools, artifacts, builds, local configuration และ logs
- [x] อัปเดต dependency patches ภายใน Angular 20; lockfile ติดตั้งด้วย npm ci ได้

## 7. ผลตรวจรับจริง

| การตรวจ | ผล |
| --- | --- |
| Backend Release build | ผ่าน — 0 warnings / 0 errors |
| Frontend production build | ผ่าน — initial bundle ประมาณ 997 kB อยู่ใน budget เดิม |
| npm ci / audit | ผ่าน — ไม่พบ vulnerabilities ณ วันที่ตรวจ |
| EF migration บน SQL Server LocalDB | ผ่าน — มี 2 ตารางธุรกิจ, FK, constraints, 8 departments / 10 employees |
| API integration | ผ่าน 46 HTTP checks สำหรับ CRUD/search/filter/validation/status/FK/CORS |
| Unexpected error | ผ่านเพิ่ม 1 check: host ทดสอบที่ config เสียคืน generic 500 ไม่เผยรายละเอียด |
| SQL Server constraints | ผ่าน 8 checks ใน transaction ที่ rollback |
| Playwright UI | ผ่าน 3 tests หลังอัปเดต dependencies รุ่นสุดท้าย |
| Visual review | ตรวจ screenshot desktop, mobile และ employee detail แล้ว |
| Git repository | สร้าง history ใหม่บน `main`; origin fetch/push เป็น URL ใหม่ และ commit เก่าไม่อยู่ใน repository นี้ |

คำสั่งปกติ:

```powershell
node scripts/test-api.mjs
sqlcmd -S '(localdb)\MSSQLLocalDB' -E -d ErpDemo -i scripts/test-database.sql -b
cd src/client
npm.cmd run test:e2e
```

API script ปกติรายงาน 46 checks; รอบตรวจ 47 checks ใช้ `ERP_FAILURE_API_URL` ชี้ไป host ทดสอบแยกที่ตั้ง connection string เป็น `Invalid` และปิด automatic migration หลังตรวจได้หยุด host ทดสอบนั้นแล้ว

ข้อจำกัดของผลทดสอบ: ทดสอบกับ LocalDB ของเครื่องนี้และ Chrome headless; ยังไม่ได้ทดสอบ SQL Server instance ระยะไกลหรือ browser ทุกค่าย

## 8. เอกสารและการส่งมอบ

- [x] README มี features ที่ทำจริง, technology, setup, database, run commands และ URLs
- [x] อธิบาย SQL Server instance ทางเลือกและการตั้ง connection string
- [x] API endpoints และตัวอย่าง request/response
- [x] อธิบาย validation, relationship และ error formats
- [x] ระบุว่าไม่มี login และ photo upload
- [x] มี demo flow และเหตุผลที่ reuse โครงสร้างเดิม
- [x] สรุปไฟล์เพิ่ม/แก้ไข/ลบและ known limitations
- [x] อัปเดต checklist ตามผลทดสอบจริง
- [x] Reinitialize Git ให้เป็น repository ใหม่บน `main` และเปลี่ยน origin

## Optional ที่ยังไม่ทำ

- [ ] Photo upload — โจทย์อนุญาตให้เป็น optional; เก็บ schema/DTO field ไว้เป็น null
- [ ] Authentication/authorization — ระบบเดิมไม่มี login ที่ต้อง reuse; demo นี้ไม่มี login
- [ ] Server-side pagination — ใช้ browser pagination สำหรับข้อมูล demo ขนาดเล็ก
- [ ] ทดสอบ remote SQL Server และ browsers อื่น — ไม่ใช่สภาพแวดล้อมที่ตรวจรอบนี้

Git history เดิมไม่อยู่ใน repository นี้แล้ว โดยสำรอง metadata เดิมไว้นอก workspace ที่ `outside the project workspace` เพื่อให้กู้คืนได้หากจำเป็น

## บันทึกการทำงาน

| วันที่ | งาน |
| --- | --- |
| 2026-09-06 | อ่านโจทย์ สำรวจโครงการ และสร้าง requirement/checklist ฉบับแรก |
| 2026-09-06 | ผู้ใช้ให้ลงมือพัฒนาและระบุ repository ใหม่ |
| 2026-09-06 | ทำ models/migration/seed, backend CRUD, frontend CRUD/dashboard, config และ cleanup |
| 2026-09-06 | ทดสอบ API/SQL/UI จริง, อัปเดต Angular patches และตรวจ build/ภาพหน้าจอ |
| 2026-09-06 | อัปเดต README, checklist และสรุปไฟล์ส่งมอบ |
| 2026-09-06 | ปรับตามคำขอให้ใช้ Migration: ลบ SQL schema script และเก็บ EF migration/seed เดิมไว้; SQL constraint test เป็นชุดทดสอบเท่านั้น |
| 2026-09-06 | Reinitialize Git บน `main` เพื่อเอา commit ของโปรเจกต์ต้นฉบับออก และตั้ง origin ไป repository ใหม่ |

เปิด demo: http://localhost:4200 — Swagger: http://localhost:5028/swagger
