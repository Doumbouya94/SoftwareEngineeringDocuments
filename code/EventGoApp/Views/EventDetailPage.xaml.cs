using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class EventDetailPage : ContentPage
{
	public EventDetailPage(EventDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}