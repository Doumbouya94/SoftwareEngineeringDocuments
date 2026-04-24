using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ProfileViewModel)?.LoadUserData();
    }
}
