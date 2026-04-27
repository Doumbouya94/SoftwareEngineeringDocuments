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
    private readonly AuthStateService _authState;
    private readonly FilterStateService _filterState;
    private string _activeFilter = "Tous";
    private string _searchText = string.Empty;

    /// <summary>Initialise le vue-modèle avec l'adaptateur d'événements.</summary>
    public HomeViewModel(IEventAdapter adapter, AuthStateService authState, FilterStateService filterState)
    {
        _adapter = adapter;
        _authState = authState;
        _filterState = filterState;
        LoadFilterPills();
    }

    /// <summary>Collection observable d'événements affichés dans le CollectionView.</summary>
    public ObservableCollection<EventViewModel> Events { get; } = new();

    /// <summary>Liste des étiquettes de filtre affichées dans la barre de filtres.</summary>
    public ObservableCollection<string> FilterPills { get; } = new();

    private void LoadFilterPills()
    {
        FilterPills.Clear();
        FilterPills.Add("Tous");

        var user = _authState.CurrentUser;
        if (user != null && user.PreferredCategories.Any())
        {
            foreach (var cat in user.PreferredCategories)
                FilterPills.Add(CategoryToFilterLabel(cat));
        }
        else
        {
            foreach (EventCategory cat in Enum.GetValues(typeof(EventCategory)))
                FilterPills.Add(CategoryToFilterLabel(cat));
        }
    }

    private static string CategoryToFilterLabel(EventCategory cat) => cat switch
    {
        EventCategory.Concerts => "Concerts",
        EventCategory.Festivals => "Festivals",
        EventCategory.Sports => "Sports",
        EventCategory.Parties => "Soirées",
        EventCategory.Food => "Gastronomie",
        EventCategory.Arts => "Arts & Culture",
        EventCategory.Outdoor => "Plein air",
        EventCategory.Networking => "Réseautage",
        _ => cat.ToString()
    };

    /// <summary>Filtre actif. Déclenche PropertyChanged lors d'un changement.</summary>
    public string ActiveFilter
    {
        get => _activeFilter;
        set
        {
            if (_activeFilter == value) return;
            _activeFilter = value;
            OnPropertyChanged();
        }
    }
    /// <summary>Texte de recherche saisi par l'utilisateur.</summary>
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;
            _searchText = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Charge les événements depuis l'adaptateur selon le filtre actif
    /// et les filtres de ville/prix du FilterStateService.
    /// </summary>
    public async Task LoadEventsAsync()
    {
        LoadFilterPills();

        if (!FilterPills.Contains(_activeFilter))
        {
            _activeFilter = "Tous";
            OnPropertyChanged(nameof(ActiveFilter));
        }

        IReadOnlyList<Event> events;

        // Si des filtres actifs existent dans FilterStateService ou qu'une catégorie est sélectionnée
        if (_filterState.HasActiveFilters || _activeFilter != "Tous")
        {
            var category = _activeFilter == "Tous"
                ? null
                : FilterLabelToCategory(_activeFilter);

            events = await _adapter.GetFilteredAsync(
                category,
                _filterState.SelectedCity,
                _filterState.MaxPrice,
                _filterState.SelectedDateFilter,
                _filterState.IsFreeOnly);
        }
        else
        {
            events = await _adapter.GetAllAsync();
        }

        Events.Clear();
        foreach (var e in events)
            Events.Add(new EventViewModel(e));
    }

    /// <summary>
    /// Recherche les événements par mot-clé en temps réel.
    /// Compatible avec les filtres de catégorie actifs.
    /// </summary>
    public async Task SearchAsync(string query)
    {
        SearchText = query;

        IReadOnlyList<Event> events;

        if (string.IsNullOrWhiteSpace(query))
        {
            // Si recherche vide, recharger les événements normalement
            await LoadEventsAsync();
            return;
        }

        // Recherche avec la catégorie active si applicable
        var category = _activeFilter == "Tous"
            ? null
            : FilterLabelToCategory(_activeFilter);

        events = await _adapter.SearchAsync(query, category);

        Events.Clear();
        foreach (var e in events)
            Events.Add(new EventViewModel(e));
    }

    /// <summary>Applique un filtre de catégorie et recharge les événements.</summary>
    public async Task ApplyFilterAsync(string filterLabel)
    {
        ActiveFilter = filterLabel;
        await LoadEventsAsync();
    }

    private static EventCategory? FilterLabelToCategory(string label) => label switch
    {
        "Concerts" => EventCategory.Concerts,
        "Festivals" => EventCategory.Festivals,
        "Sports" => EventCategory.Sports,
        "Soirées" => EventCategory.Parties,
        "Gastronomie" => EventCategory.Food,
        "Arts & Culture" => EventCategory.Arts,
        "Plein air" => EventCategory.Outdoor,
        "Réseautage" => EventCategory.Networking,
        _ => null
    };

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Notifie l'interface qu'une propriété a changé.</summary>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}