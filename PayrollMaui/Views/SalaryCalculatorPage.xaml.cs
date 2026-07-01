using PayrollMaui.Services;
using PayrollMaui.ViewModels;

namespace PayrollMaui.Views;

public partial class SalaryCalculatorPage : ContentPage
{
    private SalaryCalculatorViewModel ViewModel => BindingContext as SalaryCalculatorViewModel ?? throw new InvalidOperationException();

    public SalaryCalculatorPage()
    {
        InitializeComponent();
        var service = MauiProgram.ServiceProvider?.GetRequiredService<IEmployeeApiService>()
            ?? throw new InvalidOperationException("Payroll API service unavailable.");
        BindingContext = new SalaryCalculatorViewModel(service);
    }

    private void OnEmployeeChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is Models.EmployeeModel employee)
        {
            ViewModel.SelectedEmployeeId = employee.EmployeeID;
        }
    }
}
