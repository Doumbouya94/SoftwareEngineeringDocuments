using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

/// <summary>
/// Vue-modèle pour la page de détails d'un événement.
/// </summary>
[QueryProperty(nameof(EventId), "id")]
public partial class EventDetailViewModel : INotifyPropertyChanged
{
    private readonly IEventAdapter _adapter;
    private readonly FavoritesViewModel _favoritesViewModel;
    private Event? _event;
    private string _eventId = string.Empty;
    private bool _isBusy;
    private bool _isFavorite;

    public EventDetailViewModel(IEventAdapter adapter, FavoritesViewModel favoritesViewModel)
    {
        _adapter = adapter;
        _favoritesViewModel = favoritesViewModel;
    }

    /// <summary>Identifiant de l'événement passé en paramètre de navigation.</summary>
    public string EventId
    {
        get => _eventId;
        set
        {
            _eventId = value;
            if (Guid.TryParse(value, out var guid))
                LoadEventAsync(guid);
        }
    }

    public Event? Event
    {
        get => _event;
        set
        {
            _event = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(FormattedDate));
            OnPropertyChanged(nameof(FormattedTime));
            OnPropertyChanged(nameof(Venue));
            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(ImageSource));
            OnPropertyChanged(nameof(FormattedPrice));
            OnPropertyChanged(nameof(CategoryLabel));
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    /// <summary>Indique si l'événement est dans les favoris.</summary>
    public bool IsFavorite
    {
        get => _isFavorite;
        set
        {
            if (_isFavorite == value) return;
            _isFavorite = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(FavoriteLabel));
        }
    }

    /// <summary>Texte du bouton favori selon l'état.</summary>
    public string FavoriteLabel => _isFavorite ? "Retirer des favoris" : "Ajouter aux favoris";

    // Properties for UI binding
    public string Title => Event?.Title ?? "Chargement...";
    public string Description => Event?.Description ?? string.Empty;
    public string FormattedDate => Event?.Date.ToString("dddd, d MMMM yyyy",
        new System.Globalization.CultureInfo("fr-CA")) ?? string.Empty;
    public string FormattedTime => Event?.Date.ToString("HH:mm",
        new System.Globalization.CultureInfo("fr-CA")) ?? string.Empty;
    public string Venue => Event?.Venue ?? string.Empty;
    public string Address => Event?.Address ?? string.Empty;
    public string ImageSource => Event?.ImageSource ?? string.Empty;
    public string FormattedPrice => Event?.Price == 0 ? "Gratuit" : $"{Event?.Price:0.##} $";
    public string CategoryLabel => Event?.Category.ToString() ?? string.Empty;

    /// <summary>Bascule l'état favori de l'événement.</summary>
    public async Task ToggleFavoriteAsync()
    {
        if (_event is null) return;

        if (_isFavorite)
            await _favoritesViewModel.RemoveFavoriteAsync(_event);
        else
            await _favoritesViewModel.AddFavoriteAsync(_event);

        IsFavorite = !_isFavorite;
    }

    private async void LoadEventAsync(Guid id)
    {
        IsBusy = true;
        Event = await _adapter.GetByIdAsync(id);

        // Vérifier si l'événement est déjà dans les favoris
        if (_event is not null)
            IsFavorite = _favoritesViewModel.Favorites.Any(f => f.Id == _event.Id);

        IsBusy = false;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}