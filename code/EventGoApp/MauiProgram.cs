using Microsoft.Extensions.Logging;
using EventGoApp.Services;
using EventGoApp.ViewModels;

namespace EventGoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                
            });
        builder.Services.AddSingleton<App>();

        // SQLite 
        builder.Services.AddSingleton<SqliteService>();
        builder.Services.AddSingleton<PasswordService>();

        // Singletons
        builder.Services.AddSingleton<IAuthState, AuthStateService>();
        builder.Services.AddSingleton<LocalAuthService>();
        builder.Services.AddSingleton<OnboardingStateService>();
        builder.Services.AddSingleton<IEventAdapter, SqliteEventAdapter>();

        // ViewModels
        builder.Services.AddTransient<HomeViewModel>();

        // Pages
        builder.Services.AddTransient<Views.WelcomePage>();
        builder.Services.AddTransient<Views.LoginPage>();
        builder.Services.AddTransient<Views.RegisterPage>();
        builder.Services.AddTransient<Views.OnboardingPage>();
        builder.Services.AddTransient<Views.HomePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
