# Payroll API (ASP.NET Core)

Minimal ASP.NET Core Web API scaffold for the Payroll database using EF Core.

Requires .NET 8 SDK/runtime.

Run the API from the repository root:

```powershell
dotnet restore
dotnet run
```

Then open:

- http://localhost:5000
- http://localhost:5000/swagger

If port `5000` is already in use, run on a different port:

```powershell
dotnet run --urls http://localhost:5001
```

Update `appsettings.json` to point `DefaultConnection` at your SQL Server instance. Ensure `schema.sql` has been applied to the database.

## Blazor Frontend

The `PayrollApp` Blazor Server project is in `PayrollApp`.

```powershell
cd PayrollApp
dotnet restore
dotnet run
```

The frontend is expected to run on a separate port (typically `http://localhost:5002`). Update `PayrollApp\appsettings.json` to point `ApiBaseUrl` at your running Payroll API host and port.

> Note: `PayrollApp` depends on `DevExpress.Blazor` and may require a valid DevExpress license to build and run without evaluation warnings.

## MAUI Desktop App

The `PayrollMaui` .NET MAUI desktop app is in `PayrollMaui`.

```powershell
cd PayrollMaui
dotnet restore
dotnet run
```

This app targets Windows with .NET 8. If the API uses a non-default URL or port, update the API base address in `MauiProgram.cs`.
