-- Run only against the seeded demo database. Every operation is rolled back.
SET NOCOUNT ON;
SET XACT_ABORT OFF;
IF DB_NAME() NOT LIKE 'ErpDemo%'
    THROW 51000, 'Use a ErpDemo database for this test.', 1;
IF NOT EXISTS (SELECT 1 FROM Employees WHERE Employee_ID = 1 AND Department_ID = 1)
    THROW 51000, 'The default demo seed is required for these constraint tests.', 1;
BEGIN TRANSACTION;
BEGIN TRY
    DELETE FROM Departments WHERE Department_ID = 1;
    THROW 51000, 'FAIL: department deletion should be blocked by the FK.', 1;
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() <> 547 BEGIN ROLLBACK; THROW; END;
END CATCH;

BEGIN TRY
    INSERT INTO Employees (Department_ID, Employee_First_name, Employee_Last_Name, Gender, Date_of_Birth, Date_Joined)
    VALUES (2147483647, N'Demo', N'Test', N'Other', '1995-01-01', '2024-01-01');
    THROW 51000, 'FAIL: orphan employee should be blocked by the FK.', 1;
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() <> 547 BEGIN ROLLBACK; THROW; END;
END CATCH;

BEGIN TRY
    UPDATE Departments SET Department_Name = N'  ' WHERE Department_ID = 1;
    THROW 51000, 'FAIL: blank department name should be rejected.', 1;
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() <> 547 BEGIN ROLLBACK; THROW; END;
END CATCH;

BEGIN TRY
    UPDATE Employees SET Employee_First_name = N'  ' WHERE Employee_ID = 1;
    THROW 51000, 'FAIL: blank first name should be rejected.', 1;
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() <> 547 BEGIN ROLLBACK; THROW; END;
END CATCH;

BEGIN TRY
    UPDATE Employees SET Employee_Last_Name = N'' WHERE Employee_ID = 1;
    THROW 51000, 'FAIL: blank last name should be rejected.', 1;
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() <> 547 BEGIN ROLLBACK; THROW; END;
END CATCH;

BEGIN TRY
    UPDATE Employees SET Gender = N'Invalid' WHERE Employee_ID = 1;
    THROW 51000, 'FAIL: invalid gender should be rejected.', 1;
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() <> 547 BEGIN ROLLBACK; THROW; END;
END CATCH;

BEGIN TRY
    UPDATE Employees SET Date_Joined = '2999-01-01' WHERE Employee_ID = 1;
    THROW 51000, 'FAIL: future date joined should be rejected.', 1;
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() <> 547 BEGIN ROLLBACK; THROW; END;
END CATCH;

BEGIN TRY
    UPDATE Employees SET Date_Joined = Date_of_Birth WHERE Employee_ID = 1;
    THROW 51000, 'FAIL: date joined must be later than birth.', 1;
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() <> 547 BEGIN ROLLBACK; THROW; END;
END CATCH;
ROLLBACK TRANSACTION;
PRINT 'PASS: 8 SQL Server constraint checks; test transaction rolled back.';
