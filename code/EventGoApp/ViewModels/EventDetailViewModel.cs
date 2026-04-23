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
    private Event? _event;
    private string _eventId = string.Empty;
    private bool _isBusy;

    public EventDetailViewModel(IEventAdapter adapter)
    {
        _adapter = adapter;
    }

    /// <summary>Identifiant de l'événement passé en paramètre de navigation.</summary>
    public string EventId
    {
        get => _eventId;
        set
        {
            _eventId = value;
            if (Guid.TryParse(value, out var guid))
            {
                LoadEventAsync(guid);
            }
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
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    // Properties for UI binding
    public string Title => Event?.Title ?? "Chargement...";
    public string Description => Event?.Description ?? string.Empty;
    public string FormattedDate => Event?.Date.ToString("dddd, d MMMM yyyy", new System.Globalization.CultureInfo("fr-CA")) ?? string.Empty;
    public string FormattedTime => Event?.Date.ToString("HH:mm", new System.Globalization.CultureInfo("fr-CA")) ?? string.Empty;
    public string Venue => Event?.Venue ?? string.Empty;
    public string Address => Event?.Address ?? string.Empty;
    public string ImageSource => Event?.ImageSource ?? string.Empty;
    public string FormattedPrice => Event?.Price == 0 ? "Gratuit" : $"{Event?.Price:0.##} $";
    public string CategoryLabel => Event?.Category.ToString() ?? string.Empty;

    private async void LoadEventAsync(Guid id)
    {
        IsBusy = true;
        Event = await _adapter.GetByIdAsync(id);
        IsBusy = false;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
