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

    /// <summary>Bascule l'�tat favori depuis la page de d�tail.</summary>
    private async void OnFavoriteClicked(object sender, EventArgs e)
    {
        await _viewModel.ToggleFavoriteAsync();
    }

    private async void OnParticiperClicked(object sender, EventArgs e)
    {
        if (_viewModel.Event is null) return;
        await Shell.Current.GoToAsync($"ticketcheckout?id={_viewModel.Event.Id}");
    }
}