using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Factories;

/// <summary>
/// Représente la catégorie Concerts
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>

public class ConcertEventFactory : IEventFactory
{
    public IEnumerable<Event> CreateEvents()
    {
        var now = DateTime.UtcNow;
        return [
            new() {
                Id = Guid.NewGuid(), Title = "Festival de Jazz de Montréal",
                Description = "Le plus grand festival de jazz en Amérique du Nord vous accueille pour 11 jours de concerts gratuits et payants.",
                Date = new DateTime(2026, 6, 27, 19, 0, 0), City = "Montréal",
                Address = "175 Boulevard René-Lévesque O, Montréal, QC H2X 3P9",
                Venue = "Quartier des spectacles",
                Category = EventCategory.Concerts, Price = 0,
                ImageSource = "https://images.pexels.com/photos/167636/pexels-photo-167636.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#1A237E", IsFeatured = true,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Concert hip-hop au Centre Laval",
                Description = "Une soirée explosive avec les meilleurs artistes hip-hop québécois et canadiens.",
                Date = new DateTime(2026, 5, 30, 20, 0, 0), City = "Laval",
                Address = "1950 Rue Claude-Gagné, Laval, QC H7N 5H9",
                Venue = "Place Bell",
                Category = EventCategory.Concerts, Price = 40,
                ImageSource = "https://images.pexels.com/photos/1190297/pexels-photo-1190297.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#1A237E", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Concert classique à la Salle Maurice-O'Bready",
                Description = "L'Orchestre symphonique de Trois-Rivières présente un programme de grandes œuvres romantiques.",
                Date = new DateTime(2026, 10, 3, 20, 0, 0), City = "Trois-Rivières",
                Address = "2500 Boulevard de l'Université, Sherbrooke, QC J1K 2R1",
                Venue = "Salle Maurice-O'Bready",
                Category = EventCategory.Concerts, Price = 30,
                ImageSource = "https://images.pexels.com/photos/4028878/pexels-photo-4028878.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#1A237E", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            }
            ];
    }
}
