using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Factories;

/// <summary>
/// Représente la catégorie Sports (Événements sportifs)
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>
public class SportsEventFactory : IEventFactory
{
    public IEnumerable<Event> CreateEvents()
    {
        var now = DateTime.UtcNow;
        return
        [
            new()
            {
                Id = Guid.NewGuid(), Title = "Match des Canadiens de Montréal",
                Description = "Venez encourager le Tricolore dans ce match crucial contre les Maple Leafs de Toronto.",
                Date = new DateTime(2026, 4, 14, 19, 30, 0), City = "Montréal",
                Address = "1909 Avenue des Canadiens-de-Montréal, Montréal, QC H3B 5E8",
                Venue = "Centre Bell",
                Category = EventCategory.Sports, Price = 45,
                ImageSource = "https://images.pexels.com/photos/6469031/pexels-photo-6469031.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#B71C1C", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Match des Remparts de Québec",
                Description = "Un match de hockey junior excitant mettant en vedette les jeunes talents des Remparts.",
                Date = new DateTime(2026, 4, 28, 19, 0, 0), City = "Québec",
                Address = "250 Boulevard Wilfrid-Hamel, Québec, QC G1L 5A7",
                Venue = "Videotron Centre",
                Category = EventCategory.Sports, Price = 22,
                ImageSource = "https://images.pexels.com/photos/6469031/pexels-photo-6469031.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#B71C1C", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            },
            new()
            {
                Id = Guid.NewGuid(), Title = "Tournoi de Soccer Amateur",
                Description = "Rejoignez notre tournoi communautaire de soccer en plein air ouvert à tous les niveaux de jeu.",
                Date = new DateTime(2026, 6, 6, 8, 0, 0), City = "Laval",
                Address = "345 Avenue du Parc, Laval, QC H7E 2T7",
                Venue = "Parc de la Rivière-des-Mille-Îles",
                Category = EventCategory.Sports, Price = 0,
                ImageSource = "https://images.pexels.com/photos/274422/pexels-photo-274422.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImagePlaceholderColor = "#1B5E20", IsFeatured = false,
                CreatedAt = now, UpdatedAt = now
            }
        ];
    }
}