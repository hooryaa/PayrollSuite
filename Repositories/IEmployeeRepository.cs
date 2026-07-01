using PayrollApi.Models;

namespace PayrollApi.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        void Update(Employee employee);
        void Remove(Employee employee);
        Task<int> SaveChangesAsync();
    }
}
