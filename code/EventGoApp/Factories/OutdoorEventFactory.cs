using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Factories;

/// <summary>
/// Représente la catégorie Outdoor (Activités de plein air)
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>

public class OutdoorEventFactory : IEventFactory
{
    public IEnumerable<Event> CreateEvents()
    {
        var now = DateTime.UtcNow;
        return
        [
            new()
            {
                Id = Guid.NewGuid(), Title = "Randonnée au Parc du Mont-Royal",
                Description = "Rejoignez un groupe de randonneurs pour explorer les sentiers du mont Royal et profiter de la vue sur Montréal.",
                Date = new DateTime(2026, 5, 24, 9, 0, 0), City = "Montréal",
                Address = "1260 Chemin Remembrance, Montréal, QC H3H 1A2",
                Venue = "Parc du Mont-Royal",
                Category = EventCategory.Outdoor, Price = 0,
                ImageSource = "https://images.pexels.com/photos/1365425/pexels-photo-1365425.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#1B5E20", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Balade à vélo sur la Piste de la Capitale",
                Description = "Une promenade de 20 km en groupe sur la magnifique piste cyclable longeant la rivière des Outaouais.",
                Date = new DateTime(2026, 5, 17, 9, 0, 0), City = "Gatineau",
                Address = "100 Rue Laurier, Gatineau, QC J8X 4A6",
                Venue = "Piste de la Capitale",
                Category = EventCategory.Outdoor, Price = 0,
                ImageSource = "https://images.pexels.com/photos/3981878/pexels-photo-3981878.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#33691E", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            }
        ];
    }
}