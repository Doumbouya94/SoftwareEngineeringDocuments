using EventGoApp.Models;
using EventGoApp.ViewModels;


namespace EventGoApp.Views;

/// <summary>
/// Page des favoris. Affiche la liste des événements favoris de l'utilisateur.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
public partial class FavoritesPage : ContentPage
{
    private readonly FavoritesViewModel _viewModel;

    public FavoritesPage(FavoritesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    /// <summary>Charge les favoris à chaque apparition de la page.</summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadFavoritesAsync();
    }

    /// <summary>Retire un événement des favoris.</summary>
    private async void OnRemoveFavoriteClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Event @event)
            await _viewModel.RemoveFavoriteAsync(@event);
    }

    /// <summary>Annule le dernier geste.</summary>
    private async void OnUndoClicked(object sender, EventArgs e)
    {
        await _viewModel.UndoAsync();
    }
}