using EventGoAPI.Data;
using EventGoAPI.DTOs;
using EventGoAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventGoAPI.Services;

/// <summary>
/// Représente le service d'authentification de l'application, 
/// responsable de la gestion des utilisateurs, de la génération de tokens JWT
/// et de la réinitialisation des mots de passe.
/// </summary>
/// <remarks>
/// Ce service utilise UserManager pour gérer les utilisateurs, 
/// IConfiguration pour accéder aux paramètres de configuration,
/// et AppDbContext pour gérer les tokens de rafraîchissement dans la base de données.
/// </remarks>
public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config, AppDbContext context)
    {
        _userManager = userManager;
        _configuration = config;
        _context = context;
    }

    /// <summary>
    /// Méthode d'inscription d'un nouvel utilisateur. Crée un compte utilisateur avec les informations fournies
    /// </summary>
    /// <param name="request">Les informations de l'utilisateur à inscrire</param>
    /// <returns>Une réponse d'authentification contenant le token JWT</returns>
    /// <exception cref="Exception">Lance une exception si l'inscription échoue</exception>
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            FullName = request.FullName
        };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
        }
        return await GenerateAuthResponseAsync(user);
    }

    /// <summary>
    /// Méthode de connexion d'un utilisateur existant. 
    /// Vérifie les informations d'identification et génère un token JWT si elles sont valides.
    /// </summary>
    /// <param name="request">Les informations de connexion de l'utilisateur</param>
    /// <returns>Une réponse d'authentification contenant le token JWT</returns>
    /// <exception cref="UnauthorizedAccessException">Lance une exception si les informations d'identification sont invalides</exception>
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return await GenerateAuthResponseAsync(user);
    }

    /// <summary>
    /// Méthode de rafraîchissement du token JWT. Génère un nouveau token JWT en utilisant le refresh token fourni.
    /// </summary>
    /// <param name="refreshToken">Le refresh token de l'utilisateur</param>
    /// <returns>Une réponse d'authentification contenant le nouveau token JWT</returns>
    /// <exception cref="UnauthorizedAccessException">Lance une exception si le refresh token est invalide</exception>
    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (token == null || token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        token.IsRevoked = true;
        await _context.SaveChangesAsync();

        return await GenerateAuthResponseAsync(token.User);
    }

    /// <summary>
    /// Méthode de déconnexion d'un utilisateur. Révoque le refresh token fourni pour empêcher toute utilisation future.
    /// </summary>
    /// <param name="refreshToken">Le refresh token de l'utilisateur</param>
    /// <returns></returns>
    public async Task LogoutAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (token != null)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Méthode pour gérer la fonctionnalité de mot de passe oublié. 
    /// Génère un token de réinitialisation du mot de passe pour l'utilisateur correspondant à l'email fourni.
    /// </summary>
    /// <param name="email">L'email de l'utilisateur</param>
    /// <returns>Le token de réinitialisation du mot de passe</returns>
    public async Task<string> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return string.Empty;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // TODO: Envoyer ce token par email à l'utilisateur avec un lien vers la page de réinitialisation du mot de passe
        return token;
    }

    /// <summary>
    /// Méthode pour réinitialiser le mot de passe d'un utilisateur.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    /// <summary>
    /// Méthode privée pour générer une réponse d'authentification contenant un token 
    /// JWT et un refresh token pour un utilisateur donné.
    /// </summary>
    /// <param name="user">L'utilisateur pour lequel générer la réponse d'authentification</param>
    /// <returns>Une réponse d'authentification contenant le token JWT et le refresh token</returns>
    private async Task<AuthResponse> GenerateAuthResponseAsync(ApplicationUser user)
    {
        var (token, expiration) = GenerateJwtToken(user);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);
        return new AuthResponse(token, expiration, user.Id, user.Email!, user.UserName!, refreshToken);
    }

    /// <summary>
    /// Méthode privée pour générer un token JWT pour un utilisateur donné.
    /// </summary>
    /// <param name="user">L'utilisateur pour lequel générer le token JWT</param>
    /// <returns>Un tuple contenant le token JWT et sa date d'expiration</returns>
    private (string token, DateTime expiration) GenerateJwtToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(24); // NFR-2: 24h

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }

    /// <summary>
    /// Méthode privée pour créer un token de rafraîchissement pour un utilisateur donné.
    /// </summary>
    /// <param name="userId">L'identifiant de l'utilisateur pour lequel créer le token de rafraîchissement</param>
    /// <returns>Le token de rafraîchissement</returns>
    private async Task<string> CreateRefreshTokenAsync(Guid userId)
    {
        // revoker tous les tokens de rafraîchissement existants pour cet utilisateur
        var existing = await _context.RefreshTokens
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ToListAsync();

        foreach (var t in existing)
        {
            t.IsRevoked = true;
        }

        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(7) // NFR-2: 7 days
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken.Token;
    }
}
