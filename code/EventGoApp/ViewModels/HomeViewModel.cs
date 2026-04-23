using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

/// <summary>
/// Vue-modèle de la page d'accueil. Gère la liste des événements et les filtres de catégorie.
/// </summary>
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : Observateur — notifie l'interface des changements via INotifyPropertyChanged.
/// UserStories : US2.1 (affichage de la liste), US2.2 (filtrage par catégorie), US2.3 (filtrage par date et prix).
/// Épic : Découverte et recherche d'événements.
/// </remarks>
public partial class HomeViewModel : INotifyPropertyChanged
{
    private readonly IEventAdapter _adapter;
    private string _activeFilter = "Tous";

    /// <summary>Initialise le vue-modèle avec l'adaptateur d'événements.</summary>
    public HomeViewModel(IEventAdapter adapter)
    {
        _adapter = adapter;
    }

    /// <summary>Collection observable d'événements affichés dans le CollectionView.</summary>
    public ObservableCollection<EventViewModel> Events { get; } = new();

    /// <summary>Liste des étiquettes de filtre affichées dans la barre de filtres.</summary>
    public List<string> FilterPills { get; } = new()
    {
        "Tous", "Concerts", "Festivals", "Sports", "Soirées",
        "Gastronomie", "Arts & Culture", "Plein air", "Réseautage"
    };

    /// <summary>
    /// Filtre actif. Déclenche PropertyChanged lors d'un changement.
    /// </summary>
    public string ActiveFilter
    {
        get => _activeFilter;
        set
        {
            if (_activeFilter == value)
            {
                return;
            }

            _activeFilter = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Charge les événements depuis l'adaptateur selon le filtre actif,
    /// puis met à jour la collection Events.
    /// </summary>
    public async Task LoadEventsAsync()
    {
        IReadOnlyList<Event> events;

        if (_activeFilter == "Tous")
        {
            events = await _adapter.GetAllAsync();
        }
        else
        {
            var category = FilterLabelToCategory(_activeFilter);
            events = category.HasValue
                ? await _adapter.GetByCategoryAsync(category.Value)
                : await _adapter.GetAllAsync();
        }

        Events.Clear();
        foreach (var e in events)
        {
            Events.Add(new EventViewModel(e));
        }
    }

    /// <summary>
    /// Applique un filtre de catégorie et recharge les événements.
    /// </summary>
    /// <param name="filterLabel">Étiquette du filtre sélectionné par l'utilisateur.</param>
    public async Task ApplyFilterAsync(string filterLabel)
    {
        ActiveFilter = filterLabel;
        await LoadEventsAsync();
    }

    /// <summary>
    /// Convertit une étiquette de filtre en valeur EventCategory correspondante.
    /// Retourne null si l'étiquette ne correspond à aucune catégorie.
    /// </summary>
    private static EventCategory? FilterLabelToCategory(string label) => label switch
    {
        "Concerts"       => EventCategory.Concerts,
        "Festivals"      => EventCategory.Festivals,
        "Sports"         => EventCategory.Sports,
        "Soirées"        => EventCategory.Parties,
        "Gastronomie"    => EventCategory.Food,
        "Arts & Culture" => EventCategory.Arts,
        "Plein air"      => EventCategory.Outdoor,
        "Réseautage"     => EventCategory.Networking,
        _ => null
    };

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Notifie l'interface qu'une propriété a changé.</summary>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
