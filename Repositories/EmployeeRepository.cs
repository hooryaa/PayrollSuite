using Microsoft.EntityFrameworkCore;
using PayrollApi.Data;
using PayrollApi.Models;

namespace PayrollApi.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PayrollDbContext _db;

        public EmployeeRepository(PayrollDbContext db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<Employee> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _db.Employees.AsNoTracking().OrderBy(e => e.EmployeeID);
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _db.Employees.FindAsync(id);
        }

        public async Task AddAsync(Employee employee)
        {
            await _db.Employees.AddAsync(employee);
        }

        public void Update(Employee employee)
        {
            _db.Employees.Update(employee);
        }

        public void Remove(Employee employee)
        {
            _db.Employees.Remove(employee);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
