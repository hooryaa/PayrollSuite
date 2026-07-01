using System.Windows.Input;
using PayrollMaui.Models;
using PayrollMaui.Services;

namespace PayrollMaui.ViewModels;

public class EmployeeDetailsViewModel : BaseViewModel
{
    private readonly IEmployeeApiService _employeeApiService;

    private EmployeeModel? employee;
    public EmployeeModel? Employee
    {
        get => employee;
        set => SetProperty(ref employee, value);
    }

    public ICommand LoadCommand { get; }

    public EmployeeDetailsViewModel(IEmployeeApiService employeeApiService)
    {
        _employeeApiService = employeeApiService;
        LoadCommand = new Command<int>(async id => await LoadEmployeeAsync(id));
    }

    public async Task LoadEmployeeAsync(int id)
    {
        Employee = await _employeeApiService.GetEmployeeAsync(id);
    }
}
