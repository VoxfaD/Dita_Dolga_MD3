using Microsoft.Extensions.DependencyInjection;
using MD3t.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MD3t;

public static class MauiProgram
{
    public static IServiceProvider Services;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build(); // ņemts no 14. lekcijas video
        builder.Configuration.AddConfiguration(config);
        builder.Services.AddSingleton<IConfiguration>(config);
        // lai lapas normāli izveidotos bez problēmām ņemts no 14. lekcijas video
        builder.Services.AddTransient<MainPage>();
        builder.Services.TryAddTransient<StudentPage>();
        builder.Services.TryAddTransient<AssignementPage>();
        builder.Services.TryAddTransient<SubmissionPage>();
        // Register DbContext
        builder.Services.AddDbContext<SchoolDbContext>();

        // Build and store the service provider
        var app = builder.Build();
        Services = app.Services;

        return app;
    }
}