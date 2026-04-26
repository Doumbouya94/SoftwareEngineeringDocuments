using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Factories;

/// <summary>
/// Représente la catégorie Arts 
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>

public class ArtsEventFactory : IEventFactory
{
    public IEnumerable<Event> CreateEvents()
    {
        var now = DateTime.UtcNow;
        return
        [
            new()
            {
                Id = Guid.NewGuid(), Title = "Exposition Banksy au MAC",
                Description = "Une rétrospective immersive de l'artiste anonyme Banksy présentant ses œuvres les plus emblématiques.",
                Date = new DateTime(2026, 7, 5, 11, 0, 0), City = "Montréal",
                Address = "185 Rue Sainte-Catherine O, Montréal, QC H2X 3X5",
                Venue = "Musée d'art contemporain",
                Category = EventCategory.Arts, Price = 20,
                ImageSource = "https://images.pexels.com/photos/1839919/pexels-photo-1839919.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#006064", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Visite guidée de la Vieille-Capitale",
                Description = "Découvrez l'histoire fascinante de Québec lors d'une visite guidée à pied dans les rues du Vieux-Québec.",
                Date = new DateTime(2026, 6, 14, 10, 0, 0), City = "Québec",
                Address = "12 Rue Sainte-Anne, Québec, QC G1R 3X2",
                Venue = "Vieux-Québec",
                Category = EventCategory.Arts, Price = 18,
                ImageSource = "https://images.pexels.com/photos/372326/pexels-photo-372326.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#006064", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Festival des Arts de la Rue",
                Description = "Jongleurs, acrobates, musiciens et artistes visuels investissent le centre-ville de Sherbrooke pour un week-end festif.",
                Date = new DateTime(2026, 7, 18, 11, 0, 0), City = "Sherbrooke",
                Address = "25 Rue Wellington Nord, Sherbrooke, QC J1H 5B7",
                Venue = "Centre-ville de Sherbrooke",
                Category = EventCategory.Arts, Price = 0,
                ImageSource = "https://images.pexels.com/photos/2078826/pexels-photo-2078826.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#006064", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            }
        ];
    }
}