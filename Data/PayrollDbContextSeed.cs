using Microsoft.EntityFrameworkCore;
using PayrollApi.Models;

namespace PayrollApi.Data
{
    public static class PayrollDbContextSeed
    {
        public static async Task EnsureSeedDataAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PayrollDbContext>();

            if (!context.Database.IsInMemory())
            {
                return;
            }

            if (await context.Employees.AnyAsync())
            {
                return;
            }

            context.Employees.AddRange(
                new Employee
                {
                    EmployeeNumber = "EMP-1001",
                    FirstName = "Ayesha",
                    LastName = "Khan",
                    Email = "ayesha.khan@example.com",
                    WorkEmail = "ayesha.khan@payrollsuite.com",
                    Phone = "(555) 123-4567",
                    HireDate = DateTime.UtcNow.AddYears(-3),
                    IsActive = true,
                    DepartmentID = 4,
                    PositionID = 102,
                    EmploymentType = "FullTime",
                    BankAccount = "PK1234567890",
                    CreatedAt = DateTime.UtcNow.AddYears(-3)
                },
                new Employee
                {
                    EmployeeNumber = "EMP-1002",
                    FirstName = "Michael",
                    LastName = "Santos",
                    Email = "michael.santos@example.com",
                    WorkEmail = "michael.santos@payrollsuite.com",
                    Phone = "(555) 234-5678",
                    HireDate = DateTime.UtcNow.AddYears(-1).AddMonths(-2),
                    IsActive = true,
                    DepartmentID = 2,
                    PositionID = 201,
                    EmploymentType = "FullTime",
                    BankAccount = "PK2345678901",
                    CreatedAt = DateTime.UtcNow.AddYears(-1).AddMonths(-2)
                },
                new Employee
                {
                    EmployeeNumber = "EMP-1003",
                    FirstName = "Sara",
                    LastName = "Johnson",
                    Email = "sara.johnson@example.com",
                    WorkEmail = "sara.johnson@payrollsuite.com",
                    Phone = "(555) 345-6789",
                    HireDate = DateTime.UtcNow.AddMonths(-6),
                    IsActive = true,
                    DepartmentID = 1,
                    PositionID = 304,
                    EmploymentType = "Contract",
                    BankAccount = "PK3456789012",
                    CreatedAt = DateTime.UtcNow.AddMonths(-6)
                },
                new Employee
                {
                    EmployeeNumber = "EMP-1004",
                    FirstName = "Jonathan",
                    LastName = "Lee",
                    Email = "jonathan.lee@example.com",
                    WorkEmail = "jonathan.lee@payrollsuite.com",
                    Phone = "(555) 456-7890",
                    HireDate = DateTime.UtcNow.AddYears(-5),
                    IsActive = false,
                    DepartmentID = 5,
                    PositionID = 412,
                    EmploymentType = "PartTime",
                    BankAccount = "PK4567890123",
                    CreatedAt = DateTime.UtcNow.AddYears(-5)
                });

            await context.SaveChangesAsync();
        }
    }
}
