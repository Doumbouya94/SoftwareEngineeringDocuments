using EventGoApp.Services;
using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;
    private readonly IAuthState _authState;
    private readonly FavoritesViewModel _favoritesViewModel;

    public HomePage(HomeViewModel viewModel, IAuthState authState, FavoritesViewModel favoritesViewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authState = authState;
        _favoritesViewModel = favoritesViewModel;
        EventsCollection.ItemsSource = _viewModel.Events;
        BuildFilterPills();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        SubtitleLabel.Text = _authState.CurrentMode == AuthMode.Guest
            ? "Bienvenue, explorateur 👋 !"
            : $"Bonjour, {_authState.CurrentUser?.Username ?? "vous"} 👋 !";

        await _viewModel.LoadEventsAsync();
        await SyncFavoriteIconsAsync();
        BuildFilterPills();
    }

    /// <summary>
    /// Synchronise les icônes ❤️ / 🤍 selon les favoris existants.
    /// </summary>
    private async Task SyncFavoriteIconsAsync()
    {
        await _favoritesViewModel.LoadFavoritesAsync();
        var favoriteIds = _favoritesViewModel.Favorites
            .Select(f => f.Id)
            .ToHashSet();

        foreach (var ev in _viewModel.Events)
            ev.IsFavorite = favoriteIds.Contains(ev.Id);
    }

    private void BuildFilterPills()
    {
        FilterPillsLayout.Children.Clear();
        foreach (var label in _viewModel.FilterPills)
        {
            bool active = label == _viewModel.ActiveFilter;
            var btn = new Button
            {
                Text = label,
                FontSize = 13,
                Padding = new Thickness(14, 6),
                CornerRadius = 20,
                BackgroundColor = active ? Color.FromArgb("#6200EE") : Colors.White,
                TextColor = active ? Colors.White : Colors.Black,
                BorderColor = active ? Color.FromArgb("#6200EE") : Colors.Black,
                BorderWidth = active ? 0 : 1,
                FontAttributes = FontAttributes.Bold
            };
            btn.Clicked += async (s, e) =>
            {
                await _viewModel.ApplyFilterAsync(label);
                RefreshPillStyles();
            };
            FilterPillsLayout.Children.Add(btn);
        }
    }

    private void RefreshPillStyles()
    {
        foreach (var child in FilterPillsLayout.Children)
        {
            if (child is Button btn)
            {
                bool active = btn.Text == _viewModel.ActiveFilter;
                btn.BackgroundColor = active ? Color.FromArgb("#6200EE") : Colors.White;
                btn.TextColor = active ? Colors.White : Colors.Black;
                btn.BorderColor = active ? Color.FromArgb("#6200EE") : Colors.Black;
                btn.BorderWidth = active ? 0 : 1;
            }
        }
    }

    /// <summary>
    /// Gère le clic sur le bouton ❤️ pour ajouter ou retirer un favori.
    /// </summary>
    private async void OnFavoriteClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is EventViewModel eventVm)
        {
            if (eventVm.IsFavorite)
            {
                await _favoritesViewModel.RemoveFavoriteAsync(eventVm.Source);
                eventVm.IsFavorite = false;
            }
            else
            {
                await _favoritesViewModel.AddFavoriteAsync(eventVm.Source);
                eventVm.IsFavorite = true;
            }
        }
    }

    /// <summary>
    /// Déclenché à chaque caractère saisi dans la barre de recherche.
    /// Filtre les événements en temps réel.
    /// </summary>
    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        await _viewModel.SearchAsync(e.NewTextValue);
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Déconnexion", "Êtes-vous sûr de vouloir vous déconnecter ?", "Oui", "Non");
        if (answer)
        {
            _authState.SetLoggedOut();
            await Shell.Current.GoToAsync("//welcome");
        }
    }

    private async void OnFilterClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("filter");
    }

    private async void OnCreateEventClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("createevent");
    }

    private async void OnEventSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is EventViewModel selectedEvent)
        {
            ((CollectionView)sender).SelectedItem = null;
            await Shell.Current.GoToAsync($"eventdetails?id={selectedEvent.Id}");
        }
    }
}