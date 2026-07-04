# Payroll Suite

A comprehensive, production-ready full-stack payroll management application built with .NET 8. Payroll Suite demonstrates modern enterprise architecture with a RESTful API backend, responsive web frontend, and cross-platform desktop client—all sharing a unified codebase and business logic layer.

---

## 📋 Project Structure

```
PayrollSuite/
├── PayrollApi/          # ASP.NET Core Web API backend
├── PayrollApp/          # Blazor Server web frontend
├── PayrollMaui/         # .NET MAUI Windows desktop client
└── README.md            # This file
```

---

## 🏗️ Technology Stack

### Backend Architecture (PayrollApi)

| Technology | Version | Purpose |
|-----------|---------|---------|
| **.NET** | 8.0 | Modern, cross-platform runtime environment |
| **ASP.NET Core Web API** | 8.0 | RESTful API framework with built-in dependency injection and middleware support |
| **Entity Framework Core (EF Core)** | 8.0 | ORM (Object-Relational Mapper) for database abstraction and LINQ-to-SQL queries |
| **Microsoft.EntityFrameworkCore.SqlServer** | 8.0 | EF Core provider for SQL Server databases (production) |
| **Microsoft.EntityFrameworkCore.InMemory** | 8.0 | EF Core provider for in-memory database (development/testing) |
| **Microsoft.EntityFrameworkCore.Design** | 8.0 | Design-time tools for migrations and scaffolding |
| **Swashbuckle.AspNetCore** | 6.6.1 | Swagger/OpenAPI documentation generation and Swagger UI |

**Key Backend Features:**
- RESTful API with versioned endpoints (`/api/v1/...`)
- Global exception handling with `ProblemDetails` responses
- Request validation using data annotations (`[Required]`, `[EmailAddress]`, `[Phone]`, `[StringLength]`)
- CORS (Cross-Origin Resource Sharing) support configured from `appsettings.json`
- Structured logging with Serilog/Console providers
- Health check endpoint for monitoring
- Database migrations and seeding
- Repository pattern for data access abstraction
- Service layer for business logic

### Frontend: Web Application (PayrollApp)

| Technology | Version | Purpose |
|-----------|---------|---------|
| **.NET** | 8.0 | Runtime environment |
| **Blazor Server** | 8.0 | Server-side rendering for real-time, interactive web UI |
| **Razor Components** | 8.0 | Component-based UI framework (`.razor` files) |
| **DevExpress.Blazor** | 25.1.3 | Enterprise UI component library with grid, popup, buttons, forms, etc. |
| **DevExpress.Blazor.Themes.Fluent** | 25.1.3 | Modern Fluent design theme for DevExpress components |
| **CSS3** | Latest | Custom responsive styling with CSS variables and media queries |
| **HttpClient** | 8.0 | HTTP communication with backend API |

**Key Web Features:**
- Server-side Blazor rendering for low-latency interactivity
- Component-based architecture (`Pages/`, `Components/`, `Shared/`)
- Real-time WebSocket communication via SignalR (built into Blazor)
- DevExpress data grids with sorting, filtering, and paging
- Modal dialogs for CRUD operations
- Form validation and error handling
- Responsive design (mobile-first approach)
- Static web asset serving for CSS, JavaScript, and DevExpress libraries

### Desktop Client: Windows Application (PayrollMaui)

| Technology | Version | Purpose |
|-----------|---------|---------|
| **.NET** | 8.0 | Runtime environment |
| **.NET MAUI** | 8.0.100 | Cross-platform desktop/mobile framework (native Windows UI) |
| **XAML** | Latest | Markup language for declarative UI definition |
| **Microsoft.Maui.Controls** | 8.0.100 | MAUI UI control library |
| **Microsoft.Maui.Controls.Compatibility** | 8.0.100 | Compatibility layer for Xamarin Forms migration |
| **Microsoft.Extensions.Configuration.Json** | 8.0.0 | JSON configuration file support (`appsettings.json`) |
| **Microsoft.Extensions.Http** | 8.0.0 | HttpClient factory and management |
| **Microsoft.Extensions.Logging.Debug** | 8.0.0 | Debug logging provider |

**Key Desktop Features:**
- Native Windows UI using MAUI
- XAML-based UI definition (WPF-like)
- Shared HTTP communication with backend API
- Dependency injection via `MauiProgram`
- Configuration-driven API URLs
- Cross-platform readiness (can target macOS, Android, iOS with minimal changes)

### Database Layer

| Technology | Version | Use Case |
|-----------|---------|----------|
| **SQL Server** | 2019+ | Production database (default) |
| **Entity Framework InMemory** | 8.0 | Development, testing, and prototyping |

**Database Features:**
- Automatic migrations via EF Core
- Seed data on application startup
- Connection validation and error handling
- Configurable via `appsettings.json` (`DatabaseProvider` setting)

### Development & Documentation

| Technology | Purpose |
|-----------|---------|
| **Swagger/OpenAPI** | Interactive API documentation and testing |
| **Git** | Version control |
| **PowerShell / Terminal** | Build and run scripts |

---

## 📦 Application Layers

### Backend Layers (PayrollApi)

1. **Controllers** (`Controllers/`)
   - `EmployeesController` - HTTP endpoints for CRUD operations on employees
   - Handles request routing and response formatting

2. **Services** (`Services/`)
   - `EmployeeService` - Business logic for employee operations
   - Encapsulates domain rules and calculations
   - Dependency injected into controllers

3. **Repositories** (`Repositories/`)
   - `EmployeeRepository` - Data access logic
   - Abstracts EF Core queries with interface `IEmployeeRepository`
   - Enables testability and repository pattern benefits

4. **Data** (`Data/`)
   - `PayrollDbContext` - EF Core DbContext
   - Entity mappings and relationships
   - Database initialization and seeding

5. **Models** (`Models/`)
   - `Employee` - Domain entity representing an employee record
   - Properties: ID, Name, Email, Department, Hire Date, etc.

6. **DTOs** (`DTOs/`)
   - `EmployeeDto` - Read model for GET responses
   - `CreateEmployeeDto` - Request model for POST operations
   - `UpdateEmployeeDto` - Request model for PUT operations
   - Decouples API contracts from domain models

### Frontend Layers (PayrollApp)

1. **Pages** (`Pages/`)
   - `EmployeeList.razor` - Employee list with search, filter, and CRUD toolbar
   - `EmployeeDetails.razor` - Employee detail view and edit
   - `_Host.cshtml` - HTML layout host for Blazor Server

2. **Components** (`Components/`)
   - `EmployeeTable.razor` - Reusable employee table component
   - `EmployeeFormDialog.razor` - Modal form for create/edit
   - `Shared/` - Layout, navigation, and shared UI elements

3. **Services** (`Services/`)
   - `EmployeeApiService` - HTTP client for API communication
   - `IEmployeeApiService` - Interface for dependency injection and testability

4. **Models** (`Models/`)
   - `EmployeeViewModel` - View-specific data model for UI binding
   - `EmployeeEditModel` - Form submission model

5. **Styling** (`wwwroot/css/app.css`)
   - Global CSS variables for theming (colors, shadows, spacing)
   - DevExpress component overrides for visual consistency
   - Responsive media queries for mobile and tablet support
   - Custom classes for layout (toolbar, grids, dialogs, cards)

---

## 🚀 Getting Started

### Prerequisites

- **.NET 8 SDK** - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022** or **VS Code** with C# extension (optional but recommended)
- **SQL Server** (default) or use InMemory provider for development

### Installation & Running

#### 1. Clone and Restore

```powershell
git clone https://github.com/hooryaa/PayrollSuite.git
cd PayrollSuite
dotnet restore
```

#### 2. Configure Database (Optional)

Edit `PayrollApi/appsettings.Development.json`:

```json
{
  "DatabaseProvider": "InMemory"
}
```

Or for SQL Server:

```json
{
  "DatabaseProvider": "SqlServer",
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PayrollDb;Trusted_Connection=true;"
  }
}
```

#### 3. Run the Backend API

```powershell
cd PayrollApi
dotnet run --urls "https://localhost:5001"
```

API will be available at `https://localhost:5001`  
Swagger documentation: `https://localhost:5001/swagger`

#### 4. Run the Web Frontend

In a new terminal:

```powershell
cd PayrollApp
dotnet run --urls "http://localhost:5002"
```

Web app will be available at `http://localhost:5002`

**Note:** Ensure `PayrollApp/appsettings.json` has:

```json
{
  "ApiBaseUrl": "https://localhost:5001"
}
```

#### 5. Run the Desktop Client (Optional)

In a new terminal:

```powershell
cd PayrollMaui
dotnet run
```

**Note:** Ensure `PayrollMaui/appsettings.json` has:

```json
{
  "ApiBaseUrl": "https://localhost:5001"
}
```

---

## 📡 API Endpoints

### Base URL

`https://localhost:5001/api/v1`

### Employees

| Method | Endpoint | Description |
|--------|----------|-------------|
| **GET** | `/employees` | List all employees (supports pagination: `?page=1&pageSize=10`) |
| **GET** | `/employees/{id}` | Get employee by ID |
| **POST** | `/employees` | Create new employee (requires `CreateEmployeeDto` body) |
| **PUT** | `/employees/{id}` | Update employee (requires `UpdateEmployeeDto` body) |
| **DELETE** | `/employees/{id}` | Delete employee |

### Health & Diagnostics

| Method | Endpoint | Description |
|--------|----------|-------------|
| **GET** | `/health` | Health check status |

### API Response Format

**Success (200):**

```json
{
  "data": {
    "employeeID": 1,
    "employeeNumber": "E001",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "phone": "555-1234",
    "hireDate": "2023-01-15T00:00:00Z",
    "isActive": true,
    "departmentID": 1,
    "positionID": 1
  }
}
```

**Error (4xx/5xx):**

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "Validation failed: Email is required."
}
```

---

## 🎨 Frontend Features

### Employee List Page

- **Search Bar** - Real-time employee search by name, ID, or email
- **Department Filter** - Filter by department (HR, Finance, Operations, IT, Sales)
- **Toolbar** - Actions: New Employee, Apply Filters, Refresh
- **Data Table** - Responsive grid showing employee records
- **Actions** - View, Edit, Delete for each employee
- **Status Badge** - Active/Inactive status indicator

### Employee Form Dialog

- **Modal Popup** - Non-blocking form entry
- **Validation** - Real-time and submission-time validation
- **Fields:**
  - Employee Number (unique identifier)
  - First Name
  - Last Name
  - Email (validated email format)
  - Phone (validated phone format)
  - Hire Date (date picker)
  - Department (dropdown)
  - Position (dropdown)
  - Employment Type
  - Work Email
  - Active Status (toggle)

### Responsive Design

- **Desktop** (1200px+) - Full toolbar, multi-column layout
- **Tablet** (768px-1199px) - Adjusted spacing, single-column tables
- **Mobile** (< 768px) - Stacked layout, touch-friendly buttons

---

## 🔐 Security Features

### Input Validation

- Server-side validation on all API endpoints
- Data annotations: `[Required]`, `[StringLength]`, `[EmailAddress]`, `[Phone]`
- Model state validation middleware

### Exception Handling

- Global middleware catches unhandled exceptions
- Returns `ProblemDetails` responses in production (no sensitive stack traces)
- Detailed logging for debugging

### CORS (Cross-Origin Resource Sharing)

- Configured origins from `appsettings.json`
- Prevents unauthorized cross-origin requests

---

## 📊 Database Schema

### Employees Table

```sql
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeNumber VARCHAR(50) UNIQUE NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Phone VARCHAR(20),
    HireDate DATETIME2 NOT NULL,
    IsActive BIT DEFAULT 1,
    DepartmentID INT,
    PositionID INT,
    EmploymentType VARCHAR(50),
    WorkEmail VARCHAR(255)
);
```

---

## 🛠️ Development Workflow

### Build

```powershell
dotnet build
```

### Test

```powershell
dotnet test
```

### Migrations (EF Core)

```powershell
# Add migration
dotnet ef migrations add InitialCreate -p PayrollApi

# Apply migrations
dotnet ef database update -p PayrollApi

# Remove last migration
dotnet ef migrations remove -p PayrollApi
```

### Clean Build

```powershell
dotnet clean
dotnet build --no-incremental -c Debug
```

---

## 📝 Configuration

### appsettings.json (PayrollApi)

```json
{
  "DatabaseProvider": "InMemory|SqlServer",
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedOrigins": ["http://localhost:5002", "https://localhost:5003"]
}
```

### appsettings.json (PayrollApp)

```json
{
  "ApiBaseUrl": "https://localhost:5001"
}
```

### appsettings.json (PayrollMaui)

```json
{
  "ApiBaseUrl": "https://localhost:5001"
}
```

---

## 🎓 Architecture Patterns

### Repository Pattern

- `IEmployeeRepository` - Abstraction for data access
- `EmployeeRepository` - Concrete implementation
- Enables unit testing and loose coupling

### Service Layer

- `IEmployeeService` - Business logic interface
- `EmployeeService` - Domain logic implementation
- Dependency injected into controllers

### Dependency Injection

- Built-in ASP.NET Core DI container
- Services registered in `Startup.cs` / `Program.cs`
- Constructor injection for loose coupling

### API Versioning

- Endpoints prefixed with `/api/v1/`
- Enables future API versions without breaking clients

### DTO Pattern

- Separate request/response models from domain entities
- Prevents exposing internal structure
- Enables flexibility in API contracts

---

## 📚 Project Features & Highlights

✅ **Modern .NET 8** - Latest framework with performance optimizations  
✅ **Full-Stack Solution** - Web, API, and desktop in one repository  
✅ **Enterprise Architecture** - Layered, SOLID principles  
✅ **Responsive UI** - Mobile, tablet, and desktop support  
✅ **DevExpress Components** - Professional UI controls  
✅ **Database Flexibility** - SQL Server or InMemory  
✅ **API Documentation** - Auto-generated Swagger/OpenAPI  
✅ **Error Handling** - Global middleware with structured responses  
✅ **Configuration Management** - Environment-based `appsettings.json`  
✅ **Seed Data** - Pre-populate database on startup  
✅ **CORS Support** - Cross-origin resource sharing  
✅ **Logging** - Structured logging throughout application  
✅ **Testable Design** - Interfaces and dependency injection ready for unit tests  

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License—see the LICENSE file for details.

---

## 🔗 Resources

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/fundamentals/)
- [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-8.0#blazor-server)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui)
- [DevExpress Blazor](https://docs.devexpress.com/Blazor/401959/blazor-components)
- [Swagger/OpenAPI](https://swagger.io/)

---

## 📞 Support

For questions or issues, please open a GitHub issue or contact the development team.

---

**Happy coding! 🚀**

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