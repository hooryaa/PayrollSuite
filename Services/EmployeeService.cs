using PayrollApi.DTOs;
using PayrollApi.Models;
using PayrollApi.Repositories;

namespace PayrollApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<(IEnumerable<EmployeeDto> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _repo.GetAllAsync(pageNumber, pageSize);
            return (items.Select(MapToDto), totalCount);
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            var e = new Employee
            {
                EmployeeNumber = dto.EmployeeNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                HireDate = dto.HireDate,
                IsActive = dto.IsActive,
                DepartmentID = dto.DepartmentID,
                PositionID = dto.PositionID,
                EmploymentType = dto.EmploymentType,
                WorkEmail = dto.WorkEmail,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();

            return MapToDto(e);
        }

        public async Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.IsActive = dto.IsActive;
            existing.DepartmentID = dto.DepartmentID;
            existing.PositionID = dto.PositionID;
            existing.EmploymentType = dto.EmploymentType;

            _repo.Update(existing);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            _repo.Remove(existing);
            await _repo.SaveChangesAsync();
            return true;
        }

        private static EmployeeDto MapToDto(Employee e)
        {
            return new EmployeeDto
            {
                EmployeeID = e.EmployeeID,
                EmployeeNumber = e.EmployeeNumber,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                DepartmentID = e.DepartmentID,
                PositionID = e.PositionID,
                EmploymentType = e.EmploymentType,
                WorkEmail = e.WorkEmail
            };
        }
    }
}
