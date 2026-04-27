using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class EventDetailPage : ContentPage
{
    private readonly EventDetailViewModel _viewModel;

    public EventDetailPage(EventDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>Bascule l'état favori depuis la page de détail.</summary>
    private async void OnFavoriteClicked(object sender, EventArgs e)
    {
        await _viewModel.ToggleFavoriteAsync();
    }
}