using PayrollApi.Models;

namespace PayrollApi.Repositories
{
    public interface IEmployeeRepository
    {
        Task<(IEnumerable<Employee> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        void Update(Employee employee);
        void Remove(Employee employee);
        Task<int> SaveChangesAsync();
    }
}
