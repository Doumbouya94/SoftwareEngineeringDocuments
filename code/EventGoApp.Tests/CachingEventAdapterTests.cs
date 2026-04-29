using EventGoApp.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;
using Windows.Devices.AllJoyn;
using Windows.Devices.Bluetooth.Advertisement;

namespace EventGoApp.Tests;

/// <summary>
/// Tests unitaires pour le Patron Décorateur - CachingEventAdapter.
/// Vérifie le comportement du cache en mémoire autour de IEventAdapter.
public class CachingEventAdapterTests
{

    /// <summary>
    /// Test pour vérifier que le cache est utilisé après un appel initial à GetAllAsync.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllAsync_ShouldCall_Inner_OnCacheMiss()
    {
        var inner = Substitute.For<IEventAdapter>();
        inner.GetAllAsync().Returns(new List<Event>());
        var decorator = new CachingEventAdapter(inner);

        await decorator.GetAllAsync();

        await inner.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldNotCall_Inner_OnCacheHit()
    {
        var inner = Substitute.For<IEventAdapter>();
        inner.GetAllAsync().Returns(new List<Event>());
        var decorator = new CachingEventAdapter(inner);

        await decorator.GetAllAsync(); // premier appel - cache miss
        await decorator.GetAllAsync(); // deuxième appel - cache hit

        await inner.Received(1).GetAllAsync(); // inner doit être appelé une seule fois
    }

    /// <summary>
    /// Test pour vérifier que le cache est invalidé après un appel à AddAsync, 
    /// forçant un nouvel appel à GetAllAsync à récupérer les données mises à jour.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllAsync_ShouldCall_Inner_AfterWrite()
    {
        var inner = Substitute.For<IEventAdapter>();
        inner.GetAllAsync().Returns(new List<Event>());
        var decorator = new CachingEventAdapter(inner);

        await decorator.GetAllAsync();              // cache miss → stocké
        await decorator.AddAsync(new Event());      // cache vidé
        await decorator.GetAllAsync();              // cache miss → rappel inner

        await inner.Received(2).GetAllAsync();
    }

    /// <summary>
    /// Test pour vérifier que le cache est spécifique à chaque catégorie d'événements dans GetByCategoryAsync.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetByCategoryAsync_ShouldCache_PerCategory()
    {
        var inner = Substitute.For<IEventAdapter>();
        inner.GetByCategoryAsync(Arg.Any<EventCategory>()).Returns(new List<Event>());
        var decorator = new CachingEventAdapter(inner);

        await decorator.GetByCategoryAsync(EventCategory.Concerts);
        await decorator.GetByCategoryAsync(EventCategory.Concerts); // hit
        await decorator.GetByCategoryAsync(EventCategory.Sports);   // miss — clé différente

        await inner.Received(1).GetByCategoryAsync(EventCategory.Concerts);
        await inner.Received(1).GetByCategoryAsync(EventCategory.Sports);
    }
}
