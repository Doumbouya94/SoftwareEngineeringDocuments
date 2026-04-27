using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Interface de persistance des favoris.
/// Définit les opérations de base pour gérer les favoris d'un utilisateur.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
public interface IFavoriteRepository
{
    /// <summary>Ajoute un événement aux favoris.</summary>
    Task AddAsync(Event @event);

    /// <summary>Retire un événement des favoris par son identifiant.</summary>
    Task RemoveAsync(Guid eventId);

    /// <summary>Retourne tous les événements favoris de l'utilisateur.</summary>
    Task<IReadOnlyList<Event>> GetAllAsync();

    /// <summary>Vérifie si un événement est déjà dans les favoris.</summary>
    Task<bool> IsFavoriteAsync(Guid eventId);
}