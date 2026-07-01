using System.Windows.Input;
using PayrollMaui.Models;
using PayrollMaui.Services;

namespace PayrollMaui.ViewModels;

public class SalaryCalculatorViewModel : BaseViewModel
{
    private readonly IEmployeeApiService _employeeApiService;

    private int selectedEmployeeId;
    private decimal baseSalary;
    private decimal bonus;
    private SalaryCalculationResult? calculationResult;

    public IEnumerable<EmployeeModel> Employees { get; private set; } = Array.Empty<EmployeeModel>();
    public int SelectedEmployeeId
    {
        get => selectedEmployeeId;
        set => SetProperty(ref selectedEmployeeId, value);
    }
    public decimal BaseSalary
    {
        get => baseSalary;
        set => SetProperty(ref baseSalary, value);
    }
    public decimal Bonus
    {
        get => bonus;
        set => SetProperty(ref bonus, value);
    }
    public SalaryCalculationResult? CalculationResult
    {
        get => calculationResult;
        set => SetProperty(ref calculationResult, value);
    }

    public ICommand CalculateCommand { get; }
    public ICommand RefreshEmployeesCommand { get; }

    public SalaryCalculatorViewModel(IEmployeeApiService employeeApiService)
    {
        _employeeApiService = employeeApiService;
        CalculateCommand = new Command(async () => await CalculateSalaryAsync());
        RefreshEmployeesCommand = new Command(async () => await LoadEmployeesAsync());
    }

    public async Task LoadEmployeesAsync()
    {
        Employees = await _employeeApiService.GetEmployeesAsync();
        OnPropertyChanged(nameof(Employees));
    }

    public async Task CalculateSalaryAsync()
    {
        if (SelectedEmployeeId <= 0)
            return;

        CalculationResult = await _employeeApiService.CalculateSalaryAsync(SelectedEmployeeId, BaseSalary, Bonus);
    }
}
