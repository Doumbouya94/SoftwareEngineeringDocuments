namespace EventGoApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("login", typeof(Views.LoginPage));
		Routing.RegisterRoute("register", typeof(Views.RegisterPage));
		Routing.RegisterRoute("onboarding", typeof(Views.OnboardingPage));
		Routing.RegisterRoute("eventdetails", typeof(Views.EventDetailPage));
		Routing.RegisterRoute("filter", typeof(Views.FilterPage));
		Routing.RegisterRoute("createevent", typeof(Views.CreateEventPage));
		Routing.RegisterRoute("myevents", typeof(Views.MyEventsPage));
		Routing.RegisterRoute("editevent", typeof(Views.EditEventPage));
	}
}
