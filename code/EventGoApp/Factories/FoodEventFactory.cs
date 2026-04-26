using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Factories;

/// <summary>
/// Représente la catégorie Food (Gastronomie)
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>

public class FoodEventFactory : IEventFactory
{
    public IEnumerable<Event> CreateEvents()
    {
        var now = DateTime.UtcNow;
        return
        [
            new()
            {
                Id = Guid.NewGuid(), Title = "Brunch Gastronomique au Marché Jean-Talon",
                Description = "Découvrez les saveurs locales avec des chefs étoilés qui préparent des créations culinaires uniques.",
                Date = new DateTime(2026, 5, 10, 10, 0, 0), City = "Montréal",
                Address = "7070 Avenue Henri-Julien, Montréal, QC H2S 3S3",
                Venue = "Marché Jean-Talon",
                Category = EventCategory.Food, Price = 35,
                ImageSource = "https://images.pexels.com/photos/1640774/pexels-photo-1640774.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#F57F17", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Festival Bières et Saveurs",
                Description = "Une célébration de la gastronomie québécoise avec plus de 150 bières artisanales et des délices locaux.",
                Date = new DateTime(2026, 9, 5, 11, 0, 0), City = "Chambly",
                Address = "2 Rue de Richelieu, Chambly, QC J3L 2R2",
                Venue = "Fort Chambly",
                Category = EventCategory.Food, Price = 25,
                ImageSource = "https://images.pexels.com/photos/1267244/pexels-photo-1267244.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#F57F17", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            }
        ];
    }
}