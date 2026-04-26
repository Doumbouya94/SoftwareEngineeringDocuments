using Microsoft.Extensions.DependencyInjection;
using EventGoApp.Services;

namespace EventGoApp;

public partial class App : Application
{
    private readonly IServiceProvider _services;

	public App(IServiceProvider services)
	{
		InitializeComponent();
        _services = services;
	}

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

#if WINDOWS || MACCATALYST
        window.MaximumHeight = 900;
        window.MaximumWidth = 500;
        window.Height = 800;
        window.Width = 400;
        window.MinimumHeight = 600;
        window.MinimumWidth = 300;
#endif

        return window;
    }

    protected override async void OnStart()
    {
        base.OnStart();

        try
        {
            // 1. Initialiser la base de données SQLite
            var sqlite = _services.GetRequiredService<SqliteService>();
            await sqlite.InitializeAsync();

            // COMMENTER : Drop et recreate db
            await sqlite.DropAndRecreateAsync();

            // 2. Injecter les données de démonstration (seed)
            var auth = _services.GetRequiredService<LocalAuthService>();
            await auth.SeedDemoUserAsync();

            // 3. Injecter les événements de démonstration (Factory + Adaptateur)
            var eventSeeder = _services.GetRequiredService<IEventSeeder>();
            await eventSeeder.SeedAsync();

            // ancienne méthode de seed, remplacée par EventSeeder qui utilise les factories
            // var eventAdapter = (SqliteEventAdapter)_services.GetRequiredService<IEventAdapter>();
            // await eventAdapter.SeedEventsAsync();
        }
        catch (Exception ex)
        {
            // Log or handle error if needed
            System.Diagnostics.Debug.WriteLine($"Startup Error: {ex.Message}");
        }
    }
}