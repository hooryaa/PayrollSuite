using PayrollMaui.Models;

namespace PayrollMaui.Services;

public interface IEmployeeApiService
{
    Task<List<EmployeeModel>> GetEmployeesAsync();
    Task<EmployeeModel?> GetEmployeeAsync(int id);
    Task<SalaryCalculationResult> CalculateSalaryAsync(int employeeId, decimal baseSalary, decimal bonus);
}
