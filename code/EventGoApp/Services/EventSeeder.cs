using EventGoApp.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoApp.Services;

/// <summary>
/// Classe qui hérite de IEventSeeder et est responsable de peupler la base de données avec des événements initiaux
/// </summary>
/// <remarks>
/// Auteur : Pierre-Sylvestre Cypré
/// Patron de conception : Factory
/// UserStories :
/// Épic : Événements 
/// </remarks>


public class EventSeeder : IEventSeeder
{
    private readonly IEventAdapter _adapter;

    public EventSeeder(IEventAdapter adapter)
    {
        _adapter = adapter;
    }

    public async Task SeedAsync()
    {
       var existing = await _adapter.GetAllAsync();
        if (existing.Count > 0)
        {
            return;
        }
        var events = EventFactoryRegistry.All()
            .SelectMany(f => f.CreateEvents())
            .ToList();

        foreach (var e in events) 
        {
            await _adapter.AddAsync(e);
        }
    }
}
