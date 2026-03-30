using SQLite;
using EventGoApp.Models;

namespace EventGoApp.Services;

public class LocalAuthService
{
    private readonly SQLiteAsyncConnection _db;
    private readonly PasswordService _passwordService;

    public LocalAuthService(SqliteService sqliteService, PasswordService passwordService)
    {
        _db = sqliteService.GetConnection();
        _passwordService = passwordService;
    }

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

    public async Task<User?> RegisterAsync(
        string email, string username, string password, string? fullName = null)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email.Trim().ToLowerInvariant(),
            Username = username.Trim(),
            FullName = fullName?.Trim() ?? username.Trim(),
            PasswordHash = _passwordService.HashPassword(password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _db.InsertAsync(user);
        return user;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return await _db.Table<User>()
            .Where(u => u.Email == normalized)
            .CountAsync() > 0;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        var trimmed = username.Trim();
        return await _db.Table<User>()
            .Where(u => u.Username == trimmed)
            .CountAsync() > 0;
    }

    /// <summary>
    /// Ajoute un utilisateur de démonstration si la base de données est vide.
    /// </summary>
    public async Task SeedDemoUserAsync()
    {
        var count = await _db.Table<User>().CountAsync();
        if (count > 0)
        {
            return;
        }

        await RegisterAsync("demo@eventgo.ca", "demo", "Demo1234", "Utilisateur Démo");
    }
}
