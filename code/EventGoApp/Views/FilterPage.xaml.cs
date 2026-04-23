using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class FilterPage : ContentPage
{
	public FilterPage(FilterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
