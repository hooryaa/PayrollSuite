using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayrollMaui.Services;
using PayrollMaui.ViewModels;
using PayrollMaui.Views;

namespace PayrollMaui;

public static class MauiProgram
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        var apiBaseUrl = builder.Configuration["ApiBaseUrl"]?.Trim() ?? throw new InvalidOperationException("ApiBaseUrl is required in PayrollMaui appsettings.json.");

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { })
            .Services.AddHttpClient<IEmployeeApiService, EmployeeApiService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

        var app = builder.Build();
        ServiceProvider = app.Services;
        return app;
    }
}
