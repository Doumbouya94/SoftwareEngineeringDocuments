using EventGoApp.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Tests;

/// <summary>
/// Tests unitaires pour le patron Factory — EventFactoryRegistry et fabriques concrètes.
/// Vérifie que chaque fabrique produit des événements valides et correctement catégorisés.
/// </summary>
public class EventFactoryTests
{
    /// <summary>
    ///  Test pour vérifier que le registre des fabriques d'événements contient exactement 8 fabriques,
    ///  correspondant aux 8 catégories d'événements définies.
    /// </summary>
    [Fact]
    public void Registry_ShouldReturn_EightFactories()
    {
        var factories = EventFactoryRegistry.All();

        Assert.Equal(8, factories.Count);
    }


    /// <summary>
    /// Test pour vérifier que chaque fabrique d'événements dans le registre produit au moins un événement.
    /// </summary>
    [Fact]
    public void Registry_AllFactories_ShouldProduce_AtLeastOneEvent()
    {
        var factories = EventFactoryRegistry.All();

        foreach (var factory in factories)
        {
            Assert.NotEmpty(factory.CreateEvents());
        }
    }

    /// <summary>
    /// Test pour vérifier que les événements créés par le ConcertEventFactory appartiennent tous à la catégorie "Concerts".
    /// </summary>
    [Fact]
    public void ConcertEventFactory_ShouldProduce_OnlyConcertEvents()
    {
        var factory = new ConcertEventFactory();

        var events = factory.CreateEvents();

        Assert.All(events, e => Assert.Equal(EventCategory.Concerts, e.Category));
    }

    /// <summary>
    /// Test pour vérifier que les événements créés par le ConcertEventFactory ont des identifiants uniques.
    /// </summary>
    [Fact]
    public void ConcertEventFactory_ShouldProduce_EventsWithUniqueIds()
    {
        var factory = new ConcertEventFactory();

        var ids = factory.CreateEvents().Select(e => e.Id).ToList();

        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    /// <summary>
    /// Contract de test pour vérifier que chaque fabrique produit des événements avec la catégorie correcte.
    /// </summary>
    /// <param name="factoryType">Type de la fabrique d'événements.</param>
    /// <param name="expectedCategory">Catégorie attendue des événements produits.</param>
    [Theory]
    [InlineData(typeof(ConcertEventFactory), EventCategory.Concerts)]
    [InlineData(typeof(FestivalEventFactory), EventCategory.Festivals)]
    [InlineData(typeof(SportsEventFactory), EventCategory.Sports)]
    [InlineData(typeof(PartyEventFactory), EventCategory.Parties)]
    [InlineData(typeof(FoodEventFactory), EventCategory.Food)]
    [InlineData(typeof(ArtsEventFactory), EventCategory.Arts)]
    [InlineData(typeof(OutdoorEventFactory), EventCategory.Outdoor)]
    [InlineData(typeof(NetworkingEventFactory), EventCategory.Networking)]
    public void EachFactory_ShouldProduce_CorrectCategory(Type factoryType, EventCategory expectedCategory)
    {
        var factory = (IEventFactory)Activator.CreateInstance(factoryType)!;

        var events = factory.CreateEvents();

        Assert.All(events, e => Assert.Equal(expectedCategory, e.Category));
    }
}
