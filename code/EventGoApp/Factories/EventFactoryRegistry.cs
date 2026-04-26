using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoApp.Factories;

/// <summary>
/// Représente un registre centralisé de toutes les usines d'événements disponibles (catégories) dans l'application.
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories : 
/// Épic : Découverte et recherche d'événements.
/// </remarks>

public static class EventFactoryRegistry
{
    public static IReadOnlyList<IEventFactory> All() => 
        [
            new ConcertEventFactory(),
            new FestivalEventFactory(),
            new SportsEventFactory(),
            new PartyEventFactory(),
            new FoodEventFactory(),
            new ArtsEventFactory(),
            new OutdoorEventFactory(),
            new NetworkingEventFactory()
        ];
}
