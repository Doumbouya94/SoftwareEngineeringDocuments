using System.ComponentModel;
using System.Runtime.CompilerServices;
using EventGoApp.Models;

namespace EventGoApp.ViewModels;

/// <summary>
/// Vue-modèle observable pour un événement. Expose les propriétés formatées pour l'affichage.
/// </summary>
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : Observateur — implémente INotifyPropertyChanged pour notifier l'interface des changements.
/// UserStories : US2.1 (affichage de la liste), US2.4 (page de détails).
/// Épic : Découverte et recherche d'événements.
/// </remarks>
public class EventViewModel : INotifyPropertyChanged
{
    private readonly Event _event;
    private bool _isSelected;

    /// <summary>Initialise le vue-modèle avec l'événement source.</summary>
    public EventViewModel(Event @event)
    {
        _event = @event;
    }

    /// <summary>Identifiant unique de l'événement.</summary>
    public Guid Id => _event.Id;

    /// <summary>Titre de l'événement.</summary>
    public string Title => _event.Title;

    /// <summary>Description de l'événement.</summary>
    public string Description => _event.Description;

    /// <summary>Ville de l'événement.</summary>
    public string City => _event.City;

    /// <summary>Lieu de l'événement.</summary>
    public string Venue => _event.Venue;

    /// <summary>Chemin vers l'image de l'événement.</summary>
    public string ImageSource => _event.ImageSource;

    /// <summary>Vrai si une image est disponible pour l'événement.</summary>
    public bool HasImage => !string.IsNullOrWhiteSpace(_event.ImageSource);

    /// <summary>Couleur de fond affichée si aucune image n'est disponible.</summary>
    public string ImagePlaceholderColor => _event.ImagePlaceholderColor;

    /// <summary>Indique si l'événement est mis en vedette.</summary>
    public bool IsFeatured => _event.IsFeatured;

    /// <summary>Date formatée en français</summary>
    public string FormattedDate => _event.Date.ToString("ddd d MMM yyyy · HH:mm",
        new System.Globalization.CultureInfo("fr-CA"));

    /// <summary>Prix formaté. Retourne « Gratuit » si le prix est 0.</summary>
    public string FormattedPrice => _event.Price == 0.0 ? "Gratuit" : $"{_event.Price:0.##} $";

    /// <summary>Nom de la catégorie en français.</summary>
    public string CategoryLabel => _event.Category switch
    {
        EventCategory.Concerts   => "Concerts",
        EventCategory.Festivals  => "Festivals",
        EventCategory.Sports     => "Sports",
        EventCategory.Parties    => "Soirées",
        EventCategory.Food       => "Gastronomie",
        EventCategory.Arts       => "Arts & Culture",
        EventCategory.Outdoor    => "Plein air",
        EventCategory.Networking => "Réseautage",
        _ => _event.Category.ToString()
    };

    /// <summary>Couleur associée à la catégorie de l'événement.</summary>
    public Color CategoryColor => _event.Category switch
    {
        EventCategory.Concerts   => Color.FromArgb("#1A237E"),
        EventCategory.Festivals  => Color.FromArgb("#4A148C"),
        EventCategory.Sports     => Color.FromArgb("#B71C1C"),
        EventCategory.Parties    => Color.FromArgb("#880E4F"),
        EventCategory.Food       => Color.FromArgb("#F57F17"),
        EventCategory.Arts       => Color.FromArgb("#006064"),
        EventCategory.Outdoor    => Color.FromArgb("#1B5E20"),
        EventCategory.Networking => Color.FromArgb("#37474F"),
        _ => Color.FromArgb("#444444")
    };

    /// <summary>
    /// Indique si l'événement est sélectionné dans l'interface.
    /// Déclenche PropertyChanged pour mettre à jour l'affichage.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value)
            {
                return;
            }

            _isSelected = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Notifie l'interface qu'une propriété a changé.</summary>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
