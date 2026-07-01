using PayrollMaui.Services;
using PayrollMaui.ViewModels;

namespace PayrollMaui.Views;

[QueryProperty(nameof(EmployeeId), "employeeId")]
public partial class EmployeeDetailsPage : ContentPage
{
    private EmployeeDetailsViewModel ViewModel => BindingContext as EmployeeDetailsViewModel ?? throw new InvalidOperationException();

    public int EmployeeId
    {
        set => _ = LoadEmployeeAsync(value);
    }

    public EmployeeDetailsPage()
    {
        InitializeComponent();
        var service = MauiProgram.ServiceProvider?.GetRequiredService<IEmployeeApiService>()
            ?? throw new InvalidOperationException("Payroll API service unavailable.");
        BindingContext = new EmployeeDetailsViewModel(service);
    }

    private async Task LoadEmployeeAsync(int employeeId)
    {
        await ViewModel.LoadEmployeeAsync(employeeId);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", true);
    }
}
