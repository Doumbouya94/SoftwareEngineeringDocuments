using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Factories;

/// <summary>
/// Interface pour la création d'événements de différentes catégories (Concerts, Festivals, Arts, Food) 
/// à l'aide du patron de conception Factory.
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>

public interface IEventFactory
{
    IEnumerable<Event> CreateEvents();
}
