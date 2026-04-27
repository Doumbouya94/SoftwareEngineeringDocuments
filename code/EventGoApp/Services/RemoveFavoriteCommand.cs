using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Commande concrète pour retirer un événement des favoris.
/// Execute() retire l'événement, Undo() le rajoute.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron : Commande (GOF Comportemental)
/// </remarks>
public class RemoveFavoriteCommand : IFavoriteCommand
{
    private readonly Event _event;
    private readonly IFavoriteRepository _repo;

    public RemoveFavoriteCommand(Event @event, IFavoriteRepository repo)
    {
        _event = @event;
        _repo = repo;
    }

    /// <summary>Retire l'événement des favoris.</summary>
    public async Task Execute() => await _repo.RemoveAsync(_event.Id);

    /// <summary>Annule le retrait en rajoutant l'événement aux favoris.</summary>
    public async Task Undo() => await _repo.AddAsync(_event);
}