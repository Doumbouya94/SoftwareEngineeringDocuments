namespace EventGoApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register navigation routes (these get the back arrow automatically)
		Routing.RegisterRoute("login", typeof(Views.LoginPage));
		Routing.RegisterRoute("register", typeof(Views.RegisterPage));
	}
}
