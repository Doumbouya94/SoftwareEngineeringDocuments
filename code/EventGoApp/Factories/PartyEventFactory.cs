using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Factories;

/// <summary>
/// Représente la catégorie Parties (Soirées et événements festifs)
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>

public class PartyEventFactory : IEventFactory
{
    public IEnumerable<Event> CreateEvents()
    {
        var now = DateTime.UtcNow;
        return
        [
            new()
            {
                Id = Guid.NewGuid(), Title = "Soirée Électro au Stereo Bar",
                Description = "Une nuit de musique électronique avec des DJs locaux et internationaux dans l'une des meilleures salles de Montréal.",
                Date = new DateTime(2026, 4, 19, 22, 0, 0), City = "Montréal",
                Address = "858 Rue Sainte-Catherine Est, Montréal, QC H2L 2E3",
                Venue = "Stereo Bar",
                Category = EventCategory.Parties, Price = 15,
                ImageSource = "https://images.pexels.com/photos/1540406/pexels-photo-1540406.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#880E4F", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Soirée Salsa au Bar L'Auberge",
                Description = "Apprenez la salsa avec des instructeurs professionnels et dansez jusqu'au bout de la nuit.",
                Date = new DateTime(2026, 4, 4, 20, 0, 0), City = "Québec",
                Address = "19 Rue de l'Ancien Chantier, Québec, QC G1K 6T4",
                Venue = "Bar L'Auberge",
                Category = EventCategory.Parties, Price = 10,
                ImageSource = "https://images.pexels.com/photos/8281140/pexels-photo-8281140.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#880E4F", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            }
        ];
    }
}