using SQLite;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Gère la connexion SQLite asynchrone et l'initialisation de la base de données.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
public class SqliteService
{
    private SQLiteAsyncConnection? _db;

    /// <summary>
    /// Initialise la connexion SQLite et crée les tables si elles n'existent pas.
    /// </summary>
    public async Task InitializeAsync()
    {
        // Si déjà initialisé, on ne fait rien
        if (_db is not null)
            return;

        // Construire le chemin vers le fichier de BD
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "eventgo.db");

        // Créer la connexion
        _db = new SQLiteAsyncConnection(dbPath);

        // Créer les tables si elles n'existent pas (idempotent)
        await _db.CreateTableAsync<User>();
        await _db.CreateTableAsync<Event>();
    }

    /// <summary>
    /// Retourne la connexion active. Lance une exception si non initialisée.
    /// </summary>
    public SQLiteAsyncConnection GetConnection()
    {
        if (_db is null)
            throw new InvalidOperationException(
                "Database not initialized. Call InitializeAsync() before using services.");

        return _db;
    }

    /// <summary>
    /// Supprime et recrée les tables. À utiliser en dev/test seulement.
    /// </summary>
    public async Task DropAndRecreateAsync()
    {
        if (_db is null)
            return;

        // Supprimer les tables
        await _db.DropTableAsync<User>();
        await _db.DropTableAsync<Event>();

        // Recréer les tables
        await _db.CreateTableAsync<User>();
        await _db.CreateTableAsync<Event>();
    }
}