/* Employee Payroll Management System - SQL Server schema
   Generated: 2026-06-30
   This file defines normalized tables for Departments, Employees,
   Attendance, Payroll (pay runs + items), Leave Requests, and
   supporting lookup tables. Each table has a short explanation
   immediately following its CREATE statement.
*/

SET NOCOUNT ON;

-- ==================================================
-- Departments
-- ==================================================
CREATE TABLE dbo.Departments (
	DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
	DeptCode NVARCHAR(10) NOT NULL UNIQUE,
	DeptName NVARCHAR(100) NOT NULL,
	Location NVARCHAR(100) NULL,
	CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

/*
Why this table exists:
- Keeps company departments as a canonical list for reporting and access control.

Why each column exists:
- DepartmentID: surrogate PK for stable references.
- DeptCode: short unique code used in reports and integrations.
- DeptName: human-readable name.
- Location: optional site/campus to support multi-site payroll rules.
- CreatedAt: audit when department was added.

Why relationships exist:
- Other tables (Employees, PayRuns, etc.) reference DepartmentID to associate people and payroll entries with a department for cost-centers and reporting.

How payroll software uses it:
- Aggregate payroll by department, allocate costs, apply department-level pay policies, and restrict approvals by department.
*/

-- ==================================================
-- Positions (job titles)
-- ==================================================
CREATE TABLE dbo.Positions (
	PositionID INT IDENTITY(1,1) PRIMARY KEY,
	PositionCode NVARCHAR(20) NOT NULL UNIQUE,
	Title NVARCHAR(100) NOT NULL,
	Level NVARCHAR(50) NULL,
	CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

/*
Why this table exists:
- Normalizes job titles/positions so employees can reference a position and HR can maintain classifications.

Why each column exists:
- PositionID: PK.
- PositionCode: stable code for integrations.
- Title: job title.
- Level: e.g., "Junior", "Senior", "Manager" to help grade mapping.

Why relationships exist:
- Employees reference PositionID to indicate role for payroll rules (e.g., overtime eligibility).

How payroll software uses it:
- Determine eligibility for benefits, overtime rules, and position-based allowances.
*/

-- ==================================================
-- LeaveTypes
-- ==================================================
CREATE TABLE dbo.LeaveTypes (
	LeaveTypeID INT IDENTITY(1,1) PRIMARY KEY,
	Code NVARCHAR(20) NOT NULL UNIQUE,
	Description NVARCHAR(200) NOT NULL,
	IsPaid BIT NOT NULL DEFAULT 1,
	MaxDaysPerYear INT NULL
);

/*
Why this table exists:
- Defines canonical leave categories (sick, vacation, unpaid, etc.).

Why each column exists:
- LeaveTypeID: PK.
- Code/Description: identifiers & human text.
- IsPaid: indicates whether payroll should pay for approved time off.
- MaxDaysPerYear: policy hint used by HR/leave balance logic.

Why relationships exist:
- LeaveRequests reference LeaveTypeID to indicate the kind of leave requested.

How payroll software uses it:
- Decide whether to include leave periods in paid time; calculate unpaid deductions if IsPaid=0.
*/

-- ==================================================
-- SalaryGrades
-- ==================================================
CREATE TABLE dbo.SalaryGrades (
	SalaryGradeID INT IDENTITY(1,1) PRIMARY KEY,
	GradeCode NVARCHAR(20) NOT NULL UNIQUE,
	MinSalary DECIMAL(18,2) NOT NULL,
	MaxSalary DECIMAL(18,2) NOT NULL,
	Currency CHAR(3) NOT NULL DEFAULT 'USD'
);

/*
Why this table exists:
- Groups employees into salary bands for budgeting and payroll calculations.

Why each column exists:
- SalaryGradeID: PK.
- GradeCode: e.g., G1, MGR-1.
- Min/MaxSalary: grade boundaries for validation and budgeting.
- Currency: currency code for multinational support.

Why relationships exist:
- Employee salary history references SalaryGradeID to show the grade associated with a salary.

How payroll software uses it:
- Validate base pay, compute expected raises, and help with cost forecasting.
*/

-- ==================================================
-- Employees
-- ==================================================
CREATE TABLE dbo.Employees (
	EmployeeID INT IDENTITY(1001,1) PRIMARY KEY,
	EmployeeNumber NVARCHAR(20) NOT NULL UNIQUE,
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	MiddleName NVARCHAR(100) NULL,
	DateOfBirth DATE NULL,
	Gender CHAR(1) NULL CHECK (Gender IN ('M','F','O')),
	NationalID NVARCHAR(50) NULL UNIQUE,
	Email NVARCHAR(255) NULL UNIQUE,
	Phone NVARCHAR(30) NULL,
	HireDate DATE NOT NULL,
	TerminationDate DATE NULL,
	IsActive BIT NOT NULL DEFAULT 1,
	DepartmentID INT NULL,
	PositionID INT NULL,
	EmploymentType NVARCHAR(20) NOT NULL DEFAULT 'FullTime', -- e.g., FullTime, PartTime, Contractor
	WorkEmail NVARCHAR(255) NULL,
	BankAccount NVARCHAR(100) NULL,
	CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	CONSTRAINT FK_Employees_Department FOREIGN KEY (DepartmentID) REFERENCES dbo.Departments(DepartmentID),
	CONSTRAINT FK_Employees_Position FOREIGN KEY (PositionID) REFERENCES dbo.Positions(PositionID)
);

/*
Why this table exists:
- Core person/entity table for payroll and HR operations.

Why each column exists:
- EmployeeID: surrogate PK starting at 1001 for nicer numbering.
- EmployeeNumber: business identifier.
- Names, DOB, Gender, NationalID: identification and tax/regulatory needs.
- Email/Phone: contact info and system login identifiers.
- HireDate/TerminationDate/IsActive: employment lifecycle.
- DepartmentID/PositionID: links to cost center and role.
- EmploymentType: affects benefits and payroll calculations.
- BankAccount: pay-out destination (masked/encrypted in production).
- CreatedAt: audit.

Why relationships exist:
- DepartmentID links to Departments for cost allocation.
- PositionID links to Positions for rules/eligibility.

How payroll software uses it:
- Identify who to pay, where to deposit funds, tax jurisdictions (via NationalID or country fields), and rules based on employment type or position.
*/

-- ==================================================
-- EmployeeSalary (history)
-- ==================================================
CREATE TABLE dbo.EmployeeSalary (
	EmployeeSalaryID INT IDENTITY(1,1) PRIMARY KEY,
	EmployeeID INT NOT NULL,
	SalaryGradeID INT NULL,
	BaseSalary DECIMAL(18,2) NOT NULL,
	Currency CHAR(3) NOT NULL DEFAULT 'USD',
	EffectiveFrom DATE NOT NULL,
	EffectiveTo DATE NULL,
	IsCurrent AS (CASE WHEN EffectiveTo IS NULL OR EffectiveTo >= CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END) PERSISTED,
	CONSTRAINT FK_EmpSalary_Employee FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID),
	CONSTRAINT FK_EmpSalary_Grade FOREIGN KEY (SalaryGradeID) REFERENCES dbo.SalaryGrades(SalaryGradeID)
);

/*
Why this table exists:
- Tracks salary changes over time (audit/history), required for retro pay calculations and audits.

Why each column exists:
- EmployeeSalaryID: PK.
- EmployeeID: who the salary applies to.
- SalaryGradeID: optional link to grade at that time.
- BaseSalary: the monetary base pay.
- EffectiveFrom/To: validity window for that salary record.
- IsCurrent: computed flag to quickly find the current record.

Why relationships exist:
- References Employees and SalaryGrades for context and validation.

How payroll software uses it:
- Determine base salary for a given pay period, compute gross pay, and support retroactive adjustments when salary changes mid-period.
*/

-- ==================================================
-- Attendance
-- ==================================================
CREATE TABLE dbo.Attendance (
	AttendanceID INT IDENTITY(1,1) PRIMARY KEY,
	EmployeeID INT NOT NULL,
	WorkDate DATE NOT NULL,
	CheckIn DATETIME2 NULL,
	CheckOut DATETIME2 NULL,
	Status NVARCHAR(20) NOT NULL DEFAULT 'Present', -- Present, Absent, OnLeave, Remote
	IsApproved BIT NOT NULL DEFAULT 0,
	Notes NVARCHAR(400) NULL,
	CONSTRAINT UQ_Attendance_EmployeeDate UNIQUE (EmployeeID, WorkDate),
	CONSTRAINT FK_Attendance_Employee FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID)
);

/*
Why this table exists:
- Captures daily attendance records to compute time worked and detect absences or overtime.

Why each column exists:
- AttendanceID: PK.
- EmployeeID: who the record is for.
- WorkDate: the date being recorded.
- CheckIn/CheckOut: timestamps for time calculations.
- Status: quick classification of the day's state.
- IsApproved: manager/HR approval flag used before payroll inclusion.
- Notes: misc info (shift exceptions, manual edits).

Why relationships exist:
- Attendance references Employees to tie time records to a person for payroll calculations.

How payroll software uses it:
- Compute hours worked for hourly employees, detect unpaid absences, and feed overtime/shift differentials into pay calculations.
*/

-- ==================================================
-- LeaveRequests
-- ==================================================
CREATE TABLE dbo.LeaveRequests (
	LeaveRequestID INT IDENTITY(1,1) PRIMARY KEY,
	EmployeeID INT NOT NULL,
	LeaveTypeID INT NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	Days DECIMAL(5,2) NOT NULL,
	PartialDay NVARCHAR(20) NULL, -- AM/PM/None
	RequestDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	Status NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Rejected, Cancelled
	ApprovedBy INT NULL,
	ApprovalDate DATETIME2 NULL,
	Notes NVARCHAR(400) NULL,
	CONSTRAINT FK_Leave_Employee FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID),
	CONSTRAINT FK_Leave_Type FOREIGN KEY (LeaveTypeID) REFERENCES dbo.LeaveTypes(LeaveTypeID),
	CONSTRAINT FK_Leave_Approver FOREIGN KEY (ApprovedBy) REFERENCES dbo.Employees(EmployeeID)
);

/*
Why this table exists:
- Track employee leave requests and their approval lifecycle.

Why each column exists:
- LeaveRequestID: PK.
- EmployeeID: requestor.
- LeaveTypeID: category used to determine pay treatment.
- StartDate/EndDate/Days: period and duration.
- PartialDay: for half-day handling.
- Status/ApprovedBy/ApprovalDate: workflow metadata.
- Notes: justification or comments.

Why relationships exist:
- Links to Employees, LeaveTypes and optionally to the Approver (also an Employee).

How payroll software uses it:
- Mark attendance or exclude approved paid leaves from deductions; calculate unpaid leave deductions when Status=Approved and LeaveType.IsPaid=0.
*/

-- ==================================================
-- PayRuns (a payroll run for a period)
-- ==================================================
CREATE TABLE dbo.PayRuns (
	PayRunID INT IDENTITY(1,1) PRIMARY KEY,
	RunCode NVARCHAR(50) NOT NULL UNIQUE,
	PeriodStart DATE NOT NULL,
	PeriodEnd DATE NOT NULL,
	PayDate DATE NOT NULL,
	Status NVARCHAR(20) NOT NULL DEFAULT 'Draft', -- Draft, Locked, Processed
	CreatedBy INT NULL,
	CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	TotalGross DECIMAL(18,2) NULL,
	TotalNet DECIMAL(18,2) NULL,
	CONSTRAINT FK_PayRuns_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES dbo.Employees(EmployeeID)
);

/*
Why this table exists:
- Represents a payroll execution (monthly/biweekly) that groups employee payments.

Why each column exists:
- PayRunID/RunCode: identifiers.
- PeriodStart/End/PayDate: the payroll period and payment date.
- Status: lifecycle of run.
- CreatedBy/CreatedAt: audit.
- TotalGross/TotalNet: caching aggregated amounts for reporting.

Why relationships exist:
- CreatedBy references an employee who initiated the run for audit.

How payroll software uses it:
- Manage payroll runs, lock periods, produce payslips, and drive accounting exports.
*/

-- ==================================================
-- PayRunItems (per-employee payroll records)
-- ==================================================
CREATE TABLE dbo.PayRunItems (
	PayRunItemID INT IDENTITY(1,1) PRIMARY KEY,
	PayRunID INT NOT NULL,
	EmployeeID INT NOT NULL,
	BasicPay DECIMAL(18,2) NOT NULL DEFAULT 0.00,
	TotalEarnings DECIMAL(18,2) NOT NULL DEFAULT 0.00,
	TotalDeductions DECIMAL(18,2) NOT NULL DEFAULT 0.00,
	Tax DECIMAL(18,2) NOT NULL DEFAULT 0.00,
	NetPay DECIMAL(18,2) NOT NULL DEFAULT 0.00,
	Notes NVARCHAR(400) NULL,
	CONSTRAINT FK_PayRunItems_PayRun FOREIGN KEY (PayRunID) REFERENCES dbo.PayRuns(PayRunID) ON DELETE CASCADE,
	CONSTRAINT FK_PayRunItems_Employee FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID)
);

/*
Why this table exists:
- Holds calculated payroll totals for each employee within a PayRun.

Why each column exists:
- PayRunItemID: PK.
- PayRunID: which run this item belongs to.
- EmployeeID: who is being paid.
- BasicPay: base salary portion for the period.
- TotalEarnings/TotalDeductions/Tax/NetPay: summary fields for payslip and accounting.
- Notes: manual adjustments or comments.

Why relationships exist:
- PayRunItems reference PayRuns and Employees so that the run aggregates and per-employee details are connected.

How payroll software uses it:
- Show payslips, export payments to bank, feed accounting, and reconcile totals with PayRuns.
*/

-- ==================================================
-- PayRunItemComponents (normalized earnings/deductions per item)
-- ==================================================
CREATE TABLE dbo.PayRunItemComponents (
	ComponentID INT IDENTITY(1,1) PRIMARY KEY,
	PayRunItemID INT NOT NULL,
	ComponentType NVARCHAR(20) NOT NULL, -- Earning or Deduction
	Code NVARCHAR(50) NOT NULL, -- e.g., 'OVERTIME', 'HOUSING_ALLOW', '401K'
	Description NVARCHAR(200) NULL,
	Amount DECIMAL(18,2) NOT NULL,
	CONSTRAINT FK_Component_PayRunItem FOREIGN KEY (PayRunItemID) REFERENCES dbo.PayRunItems(PayRunItemID) ON DELETE CASCADE
);

/*
Why this table exists:
- Normalizes the breakdown of components that make up TotalEarnings and TotalDeductions.

Why each column exists:
- ComponentID: PK.
- PayRunItemID: links to the employee's pay for the run.
- ComponentType: distinguishes earnings vs deductions.
- Code/Description: identify the component for reporting and mapping to ledger accounts.
- Amount: monetary value (positive; type determines add/subtract).

Why relationships exist:
- Each component belongs to a PayRunItem so the software can enumerate components for a payslip.

How payroll software uses it:
- Produce detailed payslips, map components to GL accounts, and explain calculations to employees and auditors.
*/

-- ==================================================
-- Sample data inserts
-- ==================================================

-- Departments
INSERT INTO dbo.Departments (DeptCode, DeptName, Location) VALUES
('HR','Human Resources','Headquarters'),
('ENG','Engineering','Headquarters'),
('FIN','Finance','Headquarters'),
('SALES','Sales','Remote Office');

-- Positions
INSERT INTO dbo.Positions (PositionCode, Title, Level) VALUES
('ENG-IC','Software Engineer','IC'),
('ENG-MGR','Engineering Manager','Manager'),
('HR-ASSOC','HR Associate','IC'),
('FIN-ANALYST','Financial Analyst','IC'),
('SALES-REP','Sales Representative','IC');

-- LeaveTypes
INSERT INTO dbo.LeaveTypes (Code, Description, IsPaid, MaxDaysPerYear) VALUES
('VAC','Vacation',1,30),
('SICK','Sick Leave',1,15),
('UNPAID','Unpaid Leave',0,NULL),
('MAT','Maternity/Paternity',1,90);

-- SalaryGrades
INSERT INTO dbo.SalaryGrades (GradeCode, MinSalary, MaxSalary, Currency) VALUES
('G1',30000.00,50000.00,'USD'),
('G2',50001.00,80000.00,'USD'),
('MGR',80001.00,120000.00,'USD');

-- Employees
INSERT INTO dbo.Employees (EmployeeNumber, FirstName, LastName, DateOfBirth, Gender, NationalID, Email, Phone, HireDate, DepartmentID, PositionID, EmploymentType, WorkEmail, BankAccount)
VALUES
('E1001','Alice','Nguyen','1988-03-12','F','A123456789','alice.nguyen@example.com','+1-555-0101','2018-06-01', (SELECT DepartmentID FROM dbo.Departments WHERE DeptCode='ENG'), (SELECT PositionID FROM dbo.Positions WHERE PositionCode='ENG-IC'), 'FullTime','alice.nguyen@company.com','US123456789'),
('E1002','Bob','Smith','1985-11-20','M','B987654321','bob.smith@example.com','+1-555-0102','2016-09-15', (SELECT DepartmentID FROM dbo.Departments WHERE DeptCode='ENG'), (SELECT PositionID FROM dbo.Positions WHERE PositionCode='ENG-MGR'), 'FullTime','bob.smith@company.com','US987654321'),
('E1003','Carla','Diaz','1992-07-05','F','C234567890','carla.diaz@example.com','+1-555-0103','2019-02-01', (SELECT DepartmentID FROM dbo.Departments WHERE DeptCode='HR'), (SELECT PositionID FROM dbo.Positions WHERE PositionCode='HR-ASSOC'), 'FullTime','carla.diaz@company.com','US112233445'),
('E1004','Daniel','Khan','1990-12-30','M','D345678901','dan.khan@example.com','+1-555-0104','2020-08-10', (SELECT DepartmentID FROM dbo.Departments WHERE DeptCode='FIN'), (SELECT PositionID FROM dbo.Positions WHERE PositionCode='FIN-ANALYST'), 'FullTime','dan.khan@company.com','US998877665'),
('E1005','Eva','Brown','1995-04-21','F','E456789012','eva.brown@example.com','+1-555-0105','2021-03-12', (SELECT DepartmentID FROM dbo.Departments WHERE DeptCode='SALES'), (SELECT PositionID FROM dbo.Positions WHERE PositionCode='SALES-REP'), 'PartTime','eva.brown@company.com','US556677889');

-- EmployeeSalary (assign current salaries)
INSERT INTO dbo.EmployeeSalary (EmployeeID, SalaryGradeID, BaseSalary, Currency, EffectiveFrom)
VALUES
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1001'), (SELECT SalaryGradeID FROM dbo.SalaryGrades WHERE GradeCode='G2'), 72000.00, 'USD', '2024-01-01'),
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1002'), (SELECT SalaryGradeID FROM dbo.SalaryGrades WHERE GradeCode='MGR'), 105000.00, 'USD', '2023-07-01'),
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1003'), (SELECT SalaryGradeID FROM dbo.SalaryGrades WHERE GradeCode='G1'), 42000.00, 'USD', '2022-06-01'),
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1004'), (SELECT SalaryGradeID FROM dbo.SalaryGrades WHERE GradeCode='G2'), 65000.00, 'USD', '2024-03-01'),
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1005'), (SELECT SalaryGradeID FROM dbo.SalaryGrades WHERE GradeCode='G1'), 28000.00, 'USD', '2024-05-01');

-- Attendance sample (a few records)
INSERT INTO dbo.Attendance (EmployeeID, WorkDate, CheckIn, CheckOut, Status, IsApproved, Notes)
VALUES
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1001'),'2026-06-01','2026-06-01 08:55:00','2026-06-01 17:05:00','Present',1,NULL),
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1001'),'2026-06-02','2026-06-02 09:10:00','2026-06-02 17:00:00','Present',1,'Late 10 min'),
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1003'),'2026-06-02',NULL,NULL,'Absent',0,'Sick?'),
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1005'),'2026-06-01','2026-06-01 10:00:00','2026-06-01 14:00:00','Present',1,'Part-time shift');

-- LeaveRequests sample
INSERT INTO dbo.LeaveRequests (EmployeeID, LeaveTypeID, StartDate, EndDate, Days, PartialDay, Status, ApprovedBy, ApprovalDate, Notes)
VALUES
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1003'), (SELECT LeaveTypeID FROM dbo.LeaveTypes WHERE Code='SICK'),'2026-06-02','2026-06-02',1.0,NULL,'Pending',NULL,NULL,'Submitted via portal'),
((SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1005'), (SELECT LeaveTypeID FROM dbo.LeaveTypes WHERE Code='VAC'),'2026-07-10','2026-07-14',5.0,NULL,'Approved', (SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1002'),'2026-06-20','Manager approved');

-- Create a PayRun for June 2026 (example monthly run)
INSERT INTO dbo.PayRuns (RunCode, PeriodStart, PeriodEnd, PayDate, Status, CreatedBy)
VALUES
('PR-2026-06', '2026-06-01', '2026-06-30', '2026-07-01', 'Draft', (SELECT EmployeeID FROM dbo.Employees WHERE EmployeeNumber='E1002'));

-- Populate PayRunItems for active employees (simple pro-rated basic pay for the month)
DECLARE @PayRunID INT = (SELECT PayRunID FROM dbo.PayRuns WHERE RunCode='PR-2026-06');

INSERT INTO dbo.PayRunItems (PayRunID, EmployeeID, BasicPay, TotalEarnings, TotalDeductions, Tax, NetPay, Notes)
SELECT
	@PayRunID,
	e.EmployeeID,
	-- Basic monthly pro-rated from base salary (BaseSalary / 12)
	CAST(ISNULL(es.BaseSalary,0)/12.0 AS DECIMAL(18,2)) AS BasicPay,
	CAST(ISNULL(es.BaseSalary,0)/12.0 AS DECIMAL(18,2)) AS TotalEarnings,
	0.00 AS TotalDeductions,
	0.00 AS Tax,
	CAST(ISNULL(es.BaseSalary,0)/12.0 AS DECIMAL(18,2)) AS NetPay,
	'Auto-generated basic pay' AS Notes
FROM dbo.Employees e
LEFT JOIN dbo.EmployeeSalary es ON es.EmployeeID = e.EmployeeID AND es.IsCurrent = 1
WHERE e.IsActive = 1;

-- Add a sample overtime earning for Alice (E1001)
INSERT INTO dbo.PayRunItemComponents (PayRunItemID, ComponentType, Code, Description, Amount)
VALUES
(
	(SELECT pri.PayRunItemID FROM dbo.PayRunItems pri JOIN dbo.Employees emp ON pri.EmployeeID = emp.EmployeeID WHERE pri.PayRunID=@PayRunID AND emp.EmployeeNumber='E1001'),
	'Earning','OVERTIME','Overtime hours (4h x rate)',200.00
),
(
	(SELECT pri.PayRunItemID FROM dbo.PayRunItems pri JOIN dbo.Employees emp ON pri.EmployeeID = emp.EmployeeID WHERE pri.PayRunID=@PayRunID AND emp.EmployeeNumber='E1001'),
	'Deduction','401K','Employee 401k contribution',-150.00
);

-- Update the PayRunItems aggregates for the affected employee
UPDATE pri
SET
	TotalEarnings = TotalEarnings + ISNULL((SELECT SUM(Amount) FROM dbo.PayRunItemComponents c WHERE c.PayRunItemID=pri.PayRunItemID AND c.ComponentType='Earning'),0),
	TotalDeductions = TotalDeductions + ABS(ISNULL((SELECT SUM(Amount) FROM dbo.PayRunItemComponents c WHERE c.PayRunItemID=pri.PayRunItemID AND c.ComponentType='Deduction'),0)),
	NetPay = (BasicPay + ISNULL((SELECT SUM(Amount) FROM dbo.PayRunItemComponents c WHERE c.PayRunItemID=pri.PayRunItemID AND c.ComponentType='Earning'),0)) - ABS(ISNULL((SELECT SUM(Amount) FROM dbo.PayRunItemComponents c WHERE c.PayRunItemID=pri.PayRunItemID AND c.ComponentType='Deduction'),0)) - Tax
FROM dbo.PayRunItems pri
WHERE pri.PayRunID = @PayRunID;

-- Aggregate totals into PayRuns
UPDATE pr
SET
	TotalGross = (SELECT SUM(BasicPay + ISNULL((SELECT SUM(Amount) FROM dbo.PayRunItemComponents c WHERE c.PayRunItemID=pri.PayRunItemID AND c.ComponentType='Earning'),0)) FROM dbo.PayRunItems pri WHERE pri.PayRunID = pr.PayRunID),
	TotalNet = (SELECT SUM(NetPay) FROM dbo.PayRunItems pri WHERE pri.PayRunID = pr.PayRunID)
FROM dbo.PayRuns pr
WHERE pr.PayRunID = @PayRunID;

/*
End of schema and sample data.

Notes:
- In production, sensitive fields (BankAccount, NationalID) should be encrypted and access-controlled.
- Additional tables often used in larger systems (TaxRules, Benefits, GLMappings, EmployeeContracts, Shifts, TimeSheets) can be added following the same normalized approach.
*/

GO

