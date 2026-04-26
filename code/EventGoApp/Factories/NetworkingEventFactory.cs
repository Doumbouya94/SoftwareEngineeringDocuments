using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Factories;

/// <summary>
/// Représente la catégorie Networking (Réseautage professionnel)
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>

public class NetworkingEventFactory : IEventFactory
{
    public IEnumerable<Event> CreateEvents()
    {
        var now = DateTime.UtcNow;
        return
        [
            new()
            {
                Id = Guid.NewGuid(), Title = "Conférence Tech MTL 2026",
                Description = "La grande conférence annuelle dédiée aux nouvelles technologies, à l'IA et à l'innovation technologique.",
                Date = new DateTime(2026, 9, 15, 9, 0, 0), City = "Montréal",
                Address = "1001 Place Jean-Paul-Riopelle, Montréal, QC H2Z 1H5",
                Venue = "Palais des congrès",
                Category = EventCategory.Networking, Price = 50,
                ImageSource = "https://images.pexels.com/photos/3184465/pexels-photo-3184465.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#37474F", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Salon Carrières et Emploi",
                Description = "Rencontrez plus de 100 employeurs de la région lors de cette grande journée de réseautage professionnel.",
                Date = new DateTime(2026, 4, 21, 9, 0, 0), City = "Gatineau",
                Address = "50 Boulevard Maisonneuve, Gatineau, QC J8X 4H4",
                Venue = "Palais des congrès de Gatineau",
                Category = EventCategory.Networking, Price = 0,
                ImageSource = "https://images.pexels.com/photos/3184306/pexels-photo-3184306.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#37474F", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            }
        ];
    }
}