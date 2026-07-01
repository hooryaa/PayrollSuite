using PayrollMaui.Services;
using PayrollMaui.ViewModels;

namespace PayrollMaui.Views;

public partial class EmployeeListPage : ContentPage
{
    private EmployeeListViewModel ViewModel => BindingContext as EmployeeListViewModel ?? throw new InvalidOperationException();

    public EmployeeListPage()
    {
        InitializeComponent();
        var service = MauiProgram.ServiceProvider?.GetRequiredService<IEmployeeApiService>()
            ?? throw new InvalidOperationException("Payroll API service unavailable.");
        BindingContext = new EmployeeListViewModel(service);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.LoadEmployeesAsync();
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Models.EmployeeModel selected)
        {
            ViewModel.SelectedEmployee = selected;
            await Shell.Current.GoToAsync($"employeeDetails?employeeId={selected.EmployeeID}");
        }
    }
}
