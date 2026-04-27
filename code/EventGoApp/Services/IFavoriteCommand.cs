using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoApp.Services;

/// <summary>
/// Interface du patron Commande pour la gestion des favoris.
/// Définit les opérations d'exécution et d'annulation.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron : Commande (GOF Comportemental)
/// </remarks>
public interface IFavoriteCommand
{
    /// <summary>Exécute la commande.</summary>
    Task Execute();

    /// <summary>Annule la commande.</summary>
    Task Undo();
}