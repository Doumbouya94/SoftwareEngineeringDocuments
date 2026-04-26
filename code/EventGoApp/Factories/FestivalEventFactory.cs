using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Factories;

/// <summary>
/// Représente la catégorie Festivals
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>

public class FestivalEventFactory : IEventFactory
{
    public IEnumerable<Event> CreateEvents()
    {
        var now = DateTime.UtcNow;
        return
        [
            new()
            {
                Id = Guid.NewGuid(), Title = "Osheaga — Scène Principale",
                Description = "Trois jours de musique avec les plus grandes têtes d'affiche internationales au parc Jean-Drapeau.",
                Date = new DateTime(2026, 8, 1, 14, 0, 0), City = "Montréal",
                Address = "1 Circuit Gilles Villeneuve, Montréal, QC H3C 1A9",
                Venue = "Parc Jean-Drapeau",
                Category = EventCategory.Festivals, Price = 85,
                ImageSource = "https://images.pexels.com/photos/1105666/pexels-photo-1105666.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#4A148C", IsFeatured = true,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Festival d'Été de Québec",
                Description = "Le plus grand festival de musique en plein air au monde avec plus de 200 spectacles gratuits.",
                Date = new DateTime(2026, 7, 9, 17, 0, 0), City = "Québec",
                Address = "Plaines d'Abraham, Québec, QC G1R 5L3",
                Venue = "Plaines d'Abraham",
                Category = EventCategory.Festivals, Price = 0,
                ImageSource = "https://images.pexels.com/photos/2747446/pexels-photo-2747446.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#0D47A1", IsFeatured = true,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Festival des Montgolfières",
                Description = "Émerveillez-vous devant des dizaines de montgolfières colorées lors de ce festival unique en son genre.",
                Date = new DateTime(2026, 9, 3, 10, 0, 0), City = "Gatineau",
                Address = "801 Boulevard de la Carrière, Gatineau, QC J8Y 6T4",
                Venue = "Parc du Lac Leamy",
                Category = EventCategory.Festivals, Price = 12,
                ImageSource = "https://images.pexels.com/photos/670263/pexels-photo-670263.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#4A148C", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            }
        ];
    }
}
