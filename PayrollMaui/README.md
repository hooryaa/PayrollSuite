# PayrollMaui Desktop App

This .NET MAUI desktop application connects to the Payroll API and provides:

- Employee list browsing
- Employee details view
- Salary calculator
- REST API integration

Requires .NET 8 SDK/runtime and Windows support for MAUI.

## Architecture

The app uses MVVM:
- `Models` define the data structures returned by the API.
- `ViewModels` expose state and commands to the UI.
- `Views` are XAML pages bound to view models using data binding.

### MVVM explanation

- **Model**: `EmployeeModel` and `SalaryCalculationResult` represent API data and calculated salary results.
- **ViewModel**: `EmployeeListViewModel`, `EmployeeDetailsViewModel`, and `SalaryCalculatorViewModel` expose observable properties and `ICommand` instances for UI actions.
- **View**: XAML pages bind to view model properties and commands, keeping UI and business logic separate.

## Key patterns

- **Data Binding**: UI controls bind to view model properties, so changes in the model automatically update the view.
- **Commands**: Buttons trigger `ICommand` implementations rather than event handlers, enabling cleaner testable behavior.
- **ObservableCollection**: The employee list uses `ObservableCollection<EmployeeModel>` to notify the UI when items change.
- **Navigation**: The app uses MAUI Shell routing to move between the employee list and details pages.
- **Dependency Injection**: `HttpClient` and `IEmployeeApiService` are registered in `MauiProgram` and resolved when pages create view models.
- **HttpClient**: The API service uses `HttpClient.GetFromJsonAsync<T>` to call the Payroll API endpoints.

## Run the app

```powershell
cd PayrollMaui
dotnet restore
dotnet run
```

> Update the API base address in `MauiProgram.cs` if the Payroll API runs on a different URL.
