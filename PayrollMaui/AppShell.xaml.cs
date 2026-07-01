namespace PayrollMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("employeeDetails", typeof(Views.EmployeeDetailsPage));
    }
}
