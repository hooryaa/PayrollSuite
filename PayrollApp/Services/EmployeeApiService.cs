using System.Net.Http.Json;
using PayrollApp.Models;

namespace PayrollApp.Services
{
    public class EmployeeApiService : IEmployeeApiService
    {
        private readonly HttpClient _client;

        public EmployeeApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<EmployeeViewModel>> GetAllAsync()
        {
            return await _client.GetFromJsonAsync<List<EmployeeViewModel>>("api/employees") ?? new List<EmployeeViewModel>();
        }

        public async Task<EmployeeViewModel?> GetByIdAsync(int id)
        {
            try
            {
                return await _client.GetFromJsonAsync<EmployeeViewModel>($"api/employees/{id}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<EmployeeViewModel> CreateAsync(EmployeeEditModel createModel)
        {
            var request = new
            {
                createModel.EmployeeNumber,
                createModel.FirstName,
                createModel.LastName,
                createModel.Email,
                createModel.Phone,
                createModel.HireDate,
                createModel.IsActive,
                createModel.DepartmentID,
                createModel.PositionID,
                createModel.EmploymentType,
                createModel.WorkEmail
            };

            var response = await _client.PostAsJsonAsync("api/employees", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EmployeeViewModel>()
                ?? throw new InvalidOperationException("Failed to deserialize created employee.");
        }

        public async Task UpdateAsync(int id, EmployeeEditModel updateModel)
        {
            var request = new
            {
                updateModel.FirstName,
                updateModel.LastName,
                updateModel.Email,
                updateModel.Phone,
                updateModel.IsActive,
                updateModel.DepartmentID,
                updateModel.PositionID,
                updateModel.EmploymentType
            };

            var response = await _client.PutAsJsonAsync($"api/employees/{id}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/employees/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
