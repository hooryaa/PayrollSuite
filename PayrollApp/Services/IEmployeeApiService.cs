using PayrollApp.Models;

namespace PayrollApp.Services
{
    public interface IEmployeeApiService
    {
        Task<List<EmployeeViewModel>> GetAllAsync();
        Task<EmployeeViewModel?> GetByIdAsync(int id);
        Task<EmployeeViewModel> CreateAsync(EmployeeEditModel createModel);
        Task UpdateAsync(int id, EmployeeEditModel updateModel);
        Task DeleteAsync(int id);
    }
}
