using EventGoApp.Services;
using EventGoApp.ViewModels;

namespace EventGoApp.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;
    private readonly IAuthState _authState;

    public HomePage(HomeViewModel viewModel, IAuthState authState)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authState = authState;
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

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        _authState.SetLoggedOut();
        await Shell.Current.GoToAsync("//welcome");
    }

    private async void OnFilterClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("filter");
    }

    private async void OnEventSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is EventViewModel selectedEvent)
        {
            // Désélectionner l'item pour permettre de cliquer à nouveau
            ((CollectionView)sender).SelectedItem = null;

            // Naviguer vers la page de détails avec l'Id
            await Shell.Current.GoToAsync($"eventdetails?id={selectedEvent.Id}");
        }
    }
}
