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
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { })
            .Services.AddSingleton<IEmployeeApiService, EmployeeApiService>()
            .Services.AddHttpClient("PayrollApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
            });

        var app = builder.Build();
        ServiceProvider = app.Services;
        return app;
    }
}
