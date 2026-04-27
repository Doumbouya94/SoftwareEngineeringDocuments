using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoApp.Models;

/// <summary>
/// Représente un événement ajouté aux favoris par un utilisateur.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
[Table("Favorites")]
public class Favorite
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>Identifiant de l'utilisateur qui a ajouté le favori.</summary>
    [Indexed]
    public Guid UserId { get; set; }

    /// <summary>Identifiant de l'événement mis en favori.</summary>
    [Indexed]
    public Guid EventId { get; set; }

    /// <summary>Date à laquelle le favori a été ajouté.</summary>
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}