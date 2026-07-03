using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PayrollApi.Data;
using PayrollApi.Repositories;
using PayrollApi.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var configuration = builder.Configuration;
var databaseProvider = configuration.GetValue<string>("DatabaseProvider")?.Trim() ?? "SqlServer";
var connectionString = configuration.GetConnectionString("DefaultConnection");
var useInMemory = databaseProvider.Equals("InMemory", StringComparison.OrdinalIgnoreCase);

using var temporaryLoggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
var logger = temporaryLoggerFactory.CreateLogger<Program>();

if (!useInMemory)
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        logger.LogError("Connection string 'DefaultConnection' is missing or empty.");
        throw new InvalidOperationException("Database connection string is required when DatabaseProvider is SqlServer.");
    }

    try
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        logger.LogInformation("Successfully connected to SQL Server.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unable to connect to SQL Server with the provided connection string.");
        throw;
    }
}

// Add DbContext
builder.Services.AddDbContext<PayrollDbContext>(options =>
{
    if (useInMemory)
    {
        options.UseInMemoryDatabase("PayrollInMemory");
    }
    else
    {
        options.UseSqlServer(connectionString!);
    }
});

// Add repositories and services
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = "PayrollApi.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
        else
        {
            policy.AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = app.Environment.IsDevelopment() ? exceptionHandlerFeature?.Error?.Message : null,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        });
    });
}

app.UseCors("CorsPolicy");

var hasHttpsEndpoint = app.Urls.Any(url => url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)) ||
    !string.IsNullOrEmpty(configuration["ASPNETCORE_HTTPS_PORT"]) ||
    configuration.GetSection("Kestrel:Endpoints:Https").Exists();
if (hasHttpsEndpoint)
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => Results.Ok("Healthy"));

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<PayrollDbContext>();
if (dbContext.Database.IsInMemory())
{
    await PayrollDbContextSeed.EnsureSeedDataAsync(app.Services);
}

app.Run();
