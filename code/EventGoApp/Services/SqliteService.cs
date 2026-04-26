using SQLite;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Façade pour la gestion de la connexion SQLite et la création du schéma.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron de conception : Façade —­ cache la complexité de l'initialisation SQLite derrière deux méthodes simples.
/// UserStories : US1.1 (inscription), US1.2 (connexion), US2.1 (affichage des événements).
/// Épic : Authentification et gestion des utilisateurs / Découverte et recherche d'événements.
/// </remarks>
public class SqliteService
{
    private SQLiteAsyncConnection? _db;

    /// <summary>
    /// Initialise la connexion SQLite et crée les tables si elles n'existent pas.
    /// Méthode idempotente : sans effet si déjà appelée.
    /// </summary>
    public async Task InitializeAsync()
    {
        if (_db is not null)
        {
            return;
        }

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "eventgo.db");
        _db = new SQLiteAsyncConnection(dbPath);

        await _db.CreateTableAsync<User>();
        await _db.CreateTableAsync<Event>();
    }

    /// <summary>
    /// Retourne la connexion SQLite active.
    /// </summary>
    /// <exception cref="InvalidOperationException">Lancée si InitializeAsync() n'a pas encore été appelée.</exception>
    public SQLiteAsyncConnection GetConnection()
    {
        if (_db is null)
        {
            throw new InvalidOperationException(
                "Base de données non initialisée. Appelez InitializeAsync() avant d'utiliser les services.");
        }

        return _db;
    }

    /// <summary>
    /// Supprime et recrée toutes les tables. À utiliser en développement uniquement.
    /// </summary>
    public async Task DropAndRecreateAsync()
    {
        if (_db is null)
        {
            return;
        }

        await _db.DropTableAsync<User>();
        await _db.DropTableAsync<Event>();
        await _db.CreateTableAsync<User>();
        await _db.CreateTableAsync<Event>();
    }
}
