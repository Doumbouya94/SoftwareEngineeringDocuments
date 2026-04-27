using EventGoApp.Models;
using EventGoApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EventGoApp.ViewModels;

/// <summary>
/// Vue-modèle de la page des favoris.
/// Gère la liste des événements favoris et les commandes d'ajout, retrait et annulation.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron : Commande (GOF Comportemental) — délègue à FavoriteCommandInvoker
/// </remarks>
public class FavoritesViewModel : INotifyPropertyChanged
{
    private readonly FavoriteCommandInvoker _invoker;
    private readonly IFavoriteRepository _repo;

    public FavoritesViewModel(FavoriteCommandInvoker invoker, IFavoriteRepository repo)
    {
        _invoker = invoker;
        _repo = repo;
    }

    /// <summary>Collection observable des événements favoris.</summary>
    public ObservableCollection<Event> Favorites { get; } = new();

    /// <summary>Indique si la liste des favoris est vide.</summary>
    public bool IsEmpty => Favorites.Count == 0;

    /// <summary>
    /// Charge les favoris depuis le repository.
    /// </summary>
    public async Task LoadFavoritesAsync()
    {
        var favorites = await _repo.GetAllAsync();
        Favorites.Clear();
        foreach (var e in favorites)
            Favorites.Add(e);

        OnPropertyChanged(nameof(IsEmpty));
    }

    /// <summary>
    /// Ajoute un événement aux favoris via l'invoker.
    /// Ne contacte pas directement le repository.
    /// </summary>
    public async Task AddFavoriteAsync(Event @event)
    {
        var cmd = new AddFavoriteCommand(@event, _repo);
        await _invoker.ExecuteAsync(cmd);

        if (!Favorites.Contains(@event))
            Favorites.Add(@event);

        OnPropertyChanged(nameof(IsEmpty));
        OnPropertyChanged(nameof(CanUndo));
    }

    /// <summary>
    /// Retire un événement des favoris via l'invoker.
    /// Ne contacte pas directement le repository.
    /// </summary>
    public async Task RemoveFavoriteAsync(Event @event)
    {
        var cmd = new RemoveFavoriteCommand(@event, _repo);
        await _invoker.ExecuteAsync(cmd);

        Favorites.Remove(@event);
        OnPropertyChanged(nameof(IsEmpty));
        OnPropertyChanged(nameof(CanUndo));
    }

    /// <summary>
    /// Annule le dernier geste (ajout ou retrait).
    /// </summary>
    public async Task UndoAsync()
    {
        await _invoker.UndoLastAsync();
        await LoadFavoritesAsync();
        OnPropertyChanged(nameof(CanUndo));
    }

    /// <summary>Indique si une annulation est possible.</summary>
    public bool CanUndo => _invoker.CanUndo;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Notifie l'interface qu'une propriété a changé.</summary>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}