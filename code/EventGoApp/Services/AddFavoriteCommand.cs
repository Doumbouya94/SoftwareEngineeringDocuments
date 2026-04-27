using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Commande concrète pour ajouter un événement aux favoris.
/// Execute() ajoute l'événement, Undo() le retire.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron : Commande (GOF Comportemental)
/// </remarks>
public class AddFavoriteCommand : IFavoriteCommand
{
    private readonly Event _event;
    private readonly IFavoriteRepository _repo;

    public AddFavoriteCommand(Event @event, IFavoriteRepository repo)
    {
        _event = @event;
        _repo = repo;
    }

    /// <summary>Ajoute l'événement aux favoris.</summary>
    public async Task Execute() => await _repo.AddAsync(_event);

    /// <summary>Annule l'ajout en retirant l'événement des favoris.</summary>
    public async Task Undo() => await _repo.RemoveAsync(_event.Id);
}