# PayrollApp Blazor Server Frontend

This Blazor Server app provides a dashboard, employee list, employee details, and create/edit experience for the Payroll API.

Requires .NET 8 SDK/runtime.

## Run the frontend

```powershell
cd PayrollApp
dotnet restore
dotnet run
```

Then open the URL shown in the console (typically `http://localhost:5002`).

## Configuration

The frontend reads the API base URL from `appsettings.json`. Update `ApiBaseUrl` if the Payroll API is hosted on a different port or origin.

## DevExpress License

This app depends on `DevExpress.Blazor` and may require a valid DevExpress license to compile and run cleanly. Without a license, you may see DX1000/DX1001 warnings or build failures.

## DevExpress Components

This app uses DevExpress Blazor UI components:
- `DxGrid` for sortable, filterable, pageable employee lists
- `DxFormLayout` for a responsive data entry layout
- `DxComboBox` for improved selection controls
- `DxChart` for department staff distribution visualization

## Features

- Dashboard with KPI cards and recent hires
- Employee list with search and department filtering
- Employee details view and delete support
- Unified create/edit form for employee records
