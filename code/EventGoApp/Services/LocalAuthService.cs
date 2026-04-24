using SQLite;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Service d'authentification locale utilisant SQLite et BCrypt.
/// </summary>
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : Adaptateur — isole la logique d'authentification du reste de l'application.
/// UserStories : US1.1 (inscription), US1.2 (connexion), US1.3 (réinitialisation du mot de passe).
/// Épic : Authentification et gestion des utilisateurs.
/// </remarks>
public class LocalAuthService
{
    private readonly SQLiteAsyncConnection _db;
    private readonly PasswordService _passwordService;

    /// <summary>
    /// Initialise le service avec la connexion SQLite et le service de hachage.
    /// </summary>
    public LocalAuthService(SqliteService sqliteService, PasswordService passwordService)
    {
        _db = sqliteService.GetConnection();
        _passwordService = passwordService;
    }

    /// <summary>
    /// Authentifie un utilisateur par courriel et mot de passe.
    /// </summary>
    /// <returns>L'utilisateur si les identifiants sont valides, sinon null.</returns>
    public async Task<User?> LoginAsync(string email, string password)
    {
        var normalized = email.Trim().ToLowerInvariant();
        var user = await _db.Table<User>()
            .FirstOrDefaultAsync(u => u.Email == normalized);

        if (user is null)
        {
            return null;
        }

        return _passwordService.VerifyPassword(password, user.PasswordHash) ? user : null;
    }

    /// <summary>
    /// Crée un nouveau compte utilisateur avec le mot de passe haché.
    /// </summary>
    /// <returns>L'utilisateur créé.</returns>
    public async Task<User?> RegisterAsync(
        string email, string username, string password, string? fullName = null)
    {
        var user = new User
        {
            Id           = Guid.NewGuid(),
            Email        = email.Trim().ToLowerInvariant(),
            Username     = username.Trim(),
            FullName     = fullName?.Trim() ?? username.Trim(),
            PasswordHash = _passwordService.HashPassword(password),
            CreatedAt    = DateTime.UtcNow,
            UpdatedAt    = DateTime.UtcNow
        };

        await _db.InsertAsync(user);
        return user;
    }

    /// <summary>
    /// Vérifie si un courriel est déjà utilisé.
    /// </summary>
    public async Task<bool> EmailExistsAsync(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return await _db.Table<User>()
            .Where(u => u.Email == normalized)
            .CountAsync() > 0;
    }

    /// <summary>
    /// Vérifie si un nom d'utilisateur est déjà utilisé.
    /// </summary>
    public async Task<bool> UsernameExistsAsync(string username)
    {
        var trimmed = username.Trim();
        return await _db.Table<User>()
            .Where(u => u.Username == trimmed)
            .CountAsync() > 0;
    }

    /// <summary>
    /// Insère l'utilisateur de démonstration si la base de données est vide.
    /// Identifiants : demo@eventgo.ca / Demo1234.
    /// </summary>
    public async Task SeedDemoUserAsync()
    {
        var count = await _db.Table<User>().CountAsync();
        if (count > 0)
        {
            return;
        }

        var user = await RegisterAsync("demo@eventgo.ca", "demo", "Demo1234", "Utilisateur Démo");
        if (user != null)
        {
            user.PreferredCategories = new List<EventCategory> 
            { 
                EventCategory.Concerts, 
                EventCategory.Festivals,
                EventCategory.Arts 
            };
            await _db.UpdateAsync(user);
        }
    }
}
