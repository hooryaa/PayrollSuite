using System.Collections.ObjectModel;
using System.Windows.Input;
using PayrollMaui.Models;
using PayrollMaui.Services;

namespace PayrollMaui.ViewModels;

public class EmployeeListViewModel : BaseViewModel
{
    private readonly IEmployeeApiService _employeeApiService;

    public ObservableCollection<EmployeeModel> Employees { get; } = new();
    public ICommand RefreshCommand { get; }
    public ICommand SelectEmployeeCommand { get; }

    private EmployeeModel? selectedEmployee;
    public EmployeeModel? SelectedEmployee
    {
        get => selectedEmployee;
        set => SetProperty(ref selectedEmployee, value);
    }

    public EmployeeListViewModel(IEmployeeApiService employeeApiService)
    {
        _employeeApiService = employeeApiService;
        RefreshCommand = new Command(async () => await LoadEmployeesAsync());
        SelectEmployeeCommand = new Command(async () => await OnEmployeeSelectedAsync());
    }

    public async Task LoadEmployeesAsync()
    {
        var employees = await _employeeApiService.GetEmployeesAsync();
        Employees.Clear();
        foreach (var employee in employees)
        {
            Employees.Add(employee);
        }
    }

    private async Task OnEmployeeSelectedAsync()
    {
        if (SelectedEmployee is not null)
        {
            await Shell.Current.GoToAsync($"employeeDetails?employeeId={SelectedEmployee.EmployeeID}");
        }
    }
}
