# Payroll Suite

Payroll Suite is a portfolio-ready full-stack application built with .NET 8:

- `PayrollApi`: ASP.NET Core Web API with EF Core and SQL Server/InMemory support
- `PayrollApp`: Blazor Server frontend
- `PayrollMaui`: .NET MAUI Windows desktop client

## Tech stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (default) / InMemory provider for development
- Swagger / OpenAPI documentation
- Blazor Server UI (`PayrollApp`)
- .NET MAUI Windows client (`PayrollMaui`)

## Architecture

- `Controllers` handle HTTP API endpoints
- `Services` implement business logic
- `Repositories` abstract database access
- `DTOs` define public contracts for API requests and responses
- `Data` contains EF Core DbContext and database seed logic

## Folder structure

- `Controllers/` - API controllers
- `Data/` - EF Core context and seeding
- `DTOs/` - request and response models
- `Models/` - domain entities
- `Repositories/` - data access layer
- `Services/` - application service layer
- `PayrollApp/` - Blazor Server app
- `PayrollMaui/` - MAUI desktop app

## Getting started

From repository root:

```powershell
dotnet restore
dotnet run --project PayrollApi\PayrollApi.csproj
```

By default, the API reads configuration from `appsettings.json` and `appsettings.Development.json`.

### Run the API

```powershell
cd PayrollApi
dotnet restore
dotnet run
```

The API now exposes:

- `GET /health`
- `GET /api/v1/employees`
- `GET /api/v1/employees/{id}`
- `POST /api/v1/employees`
- `PUT /api/v1/employees/{id}`
- `DELETE /api/v1/employees/{id}`

Swagger is available in development at `/swagger`.

### Run the Blazor frontend

```powershell
cd PayrollApp
dotnet restore
dotnet run
```

Make sure `PayrollApp\appsettings.json` contains a valid `ApiBaseUrl` value for the running API host.

### Run the MAUI desktop client

```powershell
cd PayrollMaui
dotnet restore
dotnet run
```

Make sure `PayrollMaui\appsettings.json` contains a valid `ApiBaseUrl` value for the running API host.

## API improvements implemented

- request validation using `[Required]`, `[StringLength]`, `[EmailAddress]`, and `[Phone]`
- global exception middleware with `ProblemDetails` responses in production
- CORS origins configured from `appsettings.json` instead of hard-coded values
- controller versioning with `/api/v1/employees`
- simple pagination support for `GET /api/v1/employees` with `page` and `pageSize`
- logging configured in `appsettings.json`
- health check endpoint `GET /health`
- Swagger XML comments enabled for API documentation

## Running the web

1. Restore packages

```powershell
dotnet restore
```

2. Run the API

```powershell
dotnet run --project PayrollApi\PayrollApi.csproj
```

3. Run the Blazor UI

```powershell
dotnet run --project PayrollApp\PayrollApp.csproj
```

4. Run the MAUI Windows client

```powershell
dotnet run --project PayrollMaui\PayrollMaui.csproj
```

## Future improvements

- add API versioning support for additional versions
- add unit and integration tests
- add a shared DTO project or NuGet package
- add AutoMapper for object mapping
- add a deployment-ready Dockerfile
