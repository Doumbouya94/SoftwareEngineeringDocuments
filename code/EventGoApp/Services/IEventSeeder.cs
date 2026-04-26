using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoApp.Services;

/// <summary>
/// Interface pour le service de peuplement de données d'événements.
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories :
/// Épic : Événements 
/// </remarks>

public interface IEventSeeder
{
    Task SeedAsync();
}
