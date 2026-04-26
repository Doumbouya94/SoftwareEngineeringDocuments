using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class CreateEventPage : ContentPage
{
	public CreateEventPage(CreateEventViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}
