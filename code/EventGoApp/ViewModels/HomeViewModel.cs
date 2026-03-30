using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EventGoApp.Models;
using EventGoApp.Services;

namespace EventGoApp.ViewModels;

/// <summary>
/// Représente la classe de vue-modèle pour la page d'accueil, gérant la liste des événements et les filtres de catégorie.
/// </summary>
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : Observer Pattern — permet à l'interface utilisateur de réagir aux changements de propriétés
/// UserStories : US2.1 (affichage liste événements), US2.2 (filtrage par catégorie) => 
/// gère la liste des événements et les filtres de catégorie, et notifie l'interface utilisateur des changements 
/// pour mettre à jour l'affichage en conséquence.
/// Epic : "Affichage et filtrage des événements sur la page d'accueil"
/// </remarks>
///
public class HomeViewModel : INotifyPropertyChanged
{
    private readonly IEventAdapter _adapter;
    private string _activeFilter = "Tous";

    public HomeViewModel(IEventAdapter adapter)
    {
        _adapter = adapter;
    }

    public ObservableCollection<EventViewModel> Events { get; } = new();


    // Liste des étiquettes de filtres de catégorie à afficher dans l'interface utilisateur
    public List<string> FilterPills { get; } = new()
    {
        "Tous",
        "Concerts",
        "Festivals",
        "Sports",
        "Soirées",
        "Gastronomie",
        "Arts & Culture",
        "Plein air",
        "Réseautage"
    };

    // Propriété pour suivre le filtre de catégorie actif, avec notification de changement pour mettre à jour l'affichage
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
    /// Méthode qui charge les événements depuis l'adaptateur en fonction du filtre de catégorie actif,
    /// et met à jour la collection d'événements pour l'interface utilisateur.
    /// </summary>
    /// <returns></returns>
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
    /// Méthode pour appliquer un filtre de catégorie lorsque l'utilisateur sélectionne une étiquette de filtre dans 
    /// l'interface utilisateur.
    /// </summary>
    /// <param name="filterLabel">L'étiquette du filtre sélectionné par l'utilisateur.</param>
    /// <returns></returns>
    public async Task ApplyFilterAsync(string filterLabel)
    {
        ActiveFilter = filterLabel;
        await LoadEventsAsync();
    }

    /// <summary>
    /// Objet de mapping pour convertir une étiquette de filtre de catégorie en une valeur d'énumération EventCategory correspondante,
    /// </summary>
    /// <param name="label">L'étiquette de filtre de catégorie.</param>
    /// <returns>La valeur d'énumération EventCategory correspondante, ou null si l'étiquette ne correspond à aucune catégorie.</returns>
    private static EventCategory? FilterLabelToCategory(string label) => label switch
    {
        "Concerts"     => EventCategory.Concerts,
        "Festivals"    => EventCategory.Festivals,
        "Sports"       => EventCategory.Sports,
        "Soirées"      => EventCategory.Parties,
        "Gastronomie"  => EventCategory.Food,
        "Arts & Culture" => EventCategory.Arts,
        "Plein air"    => EventCategory.Outdoor,
        "Réseautage"   => EventCategory.Networking,
        _ => null
    };


    /// <summary>
    /// Evénement PropertyChanged de l'interface INotifyPropertyChanged, 
    /// utilisé pour notifier l'interface utilisateur des changements de propriétés
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
