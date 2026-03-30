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
		return new Window(new AppShell());
	}

    protected override async void OnStart()
    {
        base.OnStart();

        try
        {
            // 1. Initialize DB tables
            var sqlite = _services.GetRequiredService<SqliteService>();
            await sqlite.InitializeAsync();

            // 2. Seed demo user
            var auth = _services.GetRequiredService<LocalAuthService>();
            await auth.SeedDemoUserAsync();

            // 3. Seed sample events
            var eventAdapter = (SqliteEventAdapter)_services.GetRequiredService<IEventAdapter>();
            await eventAdapter.SeedEventsAsync();
        }
        catch (Exception ex)
        {
            // Log or handle error if needed
            System.Diagnostics.Debug.WriteLine($"Startup Error: {ex.Message}");
        }
    }
}