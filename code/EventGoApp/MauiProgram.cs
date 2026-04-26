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
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Oswald-Regular.ttf", "OswaldRegular");
                fonts.AddFont("Oswald-Bold.ttf", "OswaldBold");
                fonts.AddFont("Inter_18pt-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter_18pt-Medium.ttf", "InterMedium");
                fonts.AddFont("Inter_18pt-SemiBold.ttf", "InterSemiBold");
            });
        builder.Services.AddSingleton<App>();

        // SQLite 
        builder.Services.AddSingleton<SqliteService>();
        builder.Services.AddSingleton<PasswordService>();

        // Singletons
        builder.Services.AddSingleton<AuthStateService>();
        builder.Services.AddSingleton<IAuthState>(sp => sp.GetRequiredService<AuthStateService>());
        builder.Services.AddSingleton<LocalAuthService>();
        builder.Services.AddSingleton<OnboardingStateService>();
        builder.Services.AddSingleton<IEventAdapter, SqliteEventAdapter>();

        // ViewModels
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<EventDetailViewModel>();
        builder.Services.AddTransient<FilterViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<CreateEventViewModel>();

        // Pages
        builder.Services.AddTransient<Views.WelcomePage>();
        builder.Services.AddTransient<Views.LoginPage>();
        builder.Services.AddTransient<Views.RegisterPage>();
        builder.Services.AddTransient<Views.OnboardingPage>();
        builder.Services.AddTransient<Views.HomePage>();
        builder.Services.AddTransient<Views.EventDetailPage>();
        builder.Services.AddTransient<Views.FilterPage>();
        builder.Services.AddTransient<Views.FavoritesPage>();
        builder.Services.AddTransient<Views.TicketsPage>();
        builder.Services.AddTransient<Views.ProfilePage>();
        builder.Services.AddTransient<Views.CreateEventPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
