using System.ComponentModel;
using System.Runtime.CompilerServices;
using EventGoApp.Models;

namespace EventGoApp.ViewModels;

/// <summary>
/// Classe de vue-modèle pour représenter un événement dans l'interface utilisateur, avec des propriétés 
/// formatées pour l'affichage et une logique de sélection.
/// </summary>
/// <remarks>
/// Patron de conception : Observer Pattern — permet à l'interface utilisateur de réagir aux changements
/// de propriétés, notamment pour la sélection de l'événement.
/// UserStory : US2.1 (affichage liste événements), US2.3 (sélection événement) => fournit les données formatées pour l'affichage et gère la sélection de l'événement.
/// Elle hérite de INotifyPropertyChanged pour permettre à l'interface utilisateur de se mettre à jour 
/// automatiquement lorsque les propriétés changent, notamment pour la sélection de l'événement.
/// Auteur : Pierre
/// </remarks>
public class EventViewModel : INotifyPropertyChanged
{
    private readonly Event _event; // variable privée pour stocker l'événement d'origine
    private bool _isSelected; // variable privée pour suivre l'état de sélection de l'événement

    public EventViewModel(Event @event)
    {
        _event = @event;
    }

    // Propriétés publiques pour exposer les données de l'événement formatées pour l'affichage
    public Guid Id => _event.Id;
    public string Title => _event.Title;
    public string Description => _event.Description;
    public string City => _event.City;
    public string Venue => _event.Venue;
    public string ImageSource => _event.ImageSource;
    public bool HasImage => !string.IsNullOrWhiteSpace(_event.ImageSource);
    public string ImagePlaceholderColor => _event.ImagePlaceholderColor;
    public bool IsFeatured => _event.IsFeatured;

    public string FormattedDate => _event.Date.ToString("ddd d MMM yyyy · HH:mm",
        new System.Globalization.CultureInfo("fr-CA"));

    public string FormattedPrice => _event.Price == 0.0 ? "Gratuit" : $"{_event.Price:0.##} $";

    public string CategoryLabel => _event.Category switch
    {
        EventCategory.Concerts  => "Concerts",
        EventCategory.Festivals => "Festivals",
        EventCategory.Sports    => "Sports",
        EventCategory.Parties   => "Soirées",
        EventCategory.Food      => "Gastronomie",
        EventCategory.Arts      => "Arts & Culture",
        EventCategory.Outdoor   => "Plein air",
        EventCategory.Networking => "Réseautage",
        _ => _event.Category.ToString()
    };

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

    // Propriété pour suivre si l'événement est sélectionné dans l'interface utilisateur
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

    // Implémentation de INotifyPropertyChanged pour notifier l'interface utilisateur des changements de propriétés
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
