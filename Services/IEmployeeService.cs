using PayrollApi.DTOs;

namespace PayrollApi.Services
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<EmployeeDto> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
