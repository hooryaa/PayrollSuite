using Microsoft.EntityFrameworkCore;
using PayrollApi.Data;
using PayrollApi.Repositories;
using PayrollApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;
var databaseProvider = configuration.GetValue<string>("DatabaseProvider")?.Trim() ?? "SqlServer";

// Add DbContext
builder.Services.AddDbContext<PayrollDbContext>(options =>
{
    if (databaseProvider.Equals("InMemory", StringComparison.OrdinalIgnoreCase))
    {
        options.UseInMemoryDatabase("PayrollInMemory");
    }
    else
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing."));
    }
});

// Add repositories and services
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Allow Blazor Server front-end development against the API endpoint
const string localCorsPolicy = "LocalBlazorPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(localCorsPolicy, policy =>
        policy.WithOrigins("https://localhost:5002", "http://localhost:5002")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(localCorsPolicy);

var hasHttpsEndpoint = app.Urls.Any(url => url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)) ||
    !string.IsNullOrEmpty(builder.Configuration["ASPNETCORE_HTTPS_PORT"]) ||
    builder.Configuration.GetSection("Kestrel:Endpoints:Https").Exists();
if (hasHttpsEndpoint)
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

if (databaseProvider.Equals("InMemory", StringComparison.OrdinalIgnoreCase))
{
    await PayrollDbContextSeed.EnsureSeedDataAsync(app.Services);
}

app.Run();
