using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoApp.Services;

/// <summary>
/// Invocateur du patron Commande pour les favoris.
/// Exécute les commandes et garde un historique LIFO pour le Undo.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron : Commande (GOF Comportemental)
/// </remarks>
public class FavoriteCommandInvoker
{
    // Pile LIFO pour garder l'historique des commandes exécutées
    private readonly Stack<IFavoriteCommand> _history = new();

    /// <summary>
    /// Exécute une commande et la pousse dans l'historique.
    /// </summary>
    public async Task ExecuteAsync(IFavoriteCommand command)
    {
        await command.Execute();
        _history.Push(command);
    }

    /// <summary>
    /// Annule la dernière commande exécutée.
    /// Ne fait rien si l'historique est vide.
    /// </summary>
    public async Task UndoLastAsync()
    {
        if (_history.TryPop(out var last))
            await last.Undo();
    }

    /// <summary>Indique si l'historique contient des commandes annulables.</summary>
    public bool CanUndo => _history.Count > 0;
}