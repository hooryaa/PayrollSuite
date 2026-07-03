using System.Net.Http.Json;
using PayrollMaui.Models;

namespace PayrollMaui.Services;

public class EmployeeApiService : IEmployeeApiService
{
    private readonly HttpClient _httpClient;

    public EmployeeApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EmployeeModel>> GetEmployeesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<EmployeeModel>>("api/v1/employees")
               ?? new List<EmployeeModel>();
    }

    public async Task<EmployeeModel?> GetEmployeeAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<EmployeeModel>($"api/v1/employees/{id}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public Task<SalaryCalculationResult> CalculateSalaryAsync(int employeeId, decimal baseSalary, decimal bonus)
    {
        var tax = Math.Round((baseSalary + bonus) * 0.22m, 2);
        var benefits = Math.Round((baseSalary + bonus) * 0.12m, 2);
        return Task.FromResult(new SalaryCalculationResult
        {
            BaseSalary = baseSalary,
            TaxAmount = tax,
            Benefits = benefits,
            NetSalary = Math.Round(baseSalary + bonus - tax + benefits, 2)
        });
    }
}
