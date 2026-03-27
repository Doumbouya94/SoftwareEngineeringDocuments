using Microsoft.AspNetCore.Mvc;
using EventGoAPI.DTOs;
using EventGoAPI.Services;

namespace EventGoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

/// <summary>
/// Classe AuthController qui gère les endpoints d'authentification de l'application,
/// tels que l'inscription, la connexion, la déconnexion, le rafraîchissement du token et la réinitialisation du mot de passe.
/// </summary>
/// <remarks>
/// Cette classe utilise le service AuthService pour gérer les opérations d'authentification.
/// </remarks>
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Endpoint pour l'inscription d'un nouvel utilisateur.
    /// </summary>
    /// <param name="request">Les informations de l'utilisateur à inscrire.</param>
    /// <returns>Le profil de l'utilisateur inscrit.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Endpoint pour la connexion d'un utilisateur existant. 
    /// Vérifie les informations d'identification et génère un token JWT si elles sont valides.
    /// </summary>
    /// <param name="request">Les informations de connexion de l'utilisateur.</param>
    /// <returns>Le token JWT si les informations d'identification sont valides.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Endpoint pour le rafraîchissement du token JWT. Génère un nouveau token JWT en utilisant le refresh token fourni.
    /// </summary>
    /// <param name="request">Les informations du refresh token.</param>
    /// <returns>Le nouveau token JWT.</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        try
        {
            return Ok(await _authService.RefreshTokenAsync(request.RefreshToken));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Endpoint pour la déconnexion d'un utilisateur, 
    /// qui révoque le token de rafraîchissement associé pour empêcher toute utilisation future.
    /// </summary>
    /// <param name="request">Les informations du refresh token à révoquer.</param>
    /// <returns>Une réponse indiquant que la déconnexion a été effectuée avec succès.</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        await _authService.LogoutAsync(request.RefreshToken);
        return NoContent();
    }

    /// <summary>
    /// Endpoint pour la fonctionnalité de mot de passe oublié.
    /// </summary>
    /// <param name="request">Les informations de l'utilisateur pour la réinitialisation du mot de passe.</param>
    /// <returns>Une réponse indiquant que le token de réinitialisation a été envoyé si le compte existe.</returns>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var token = await _authService.ForgotPasswordAsync(request.Email);
        // Toujours 200 — évite l'énumération des emails (NFR-2)
        // Token retourné directement. en production il serait envoyé par email,
        // mais pour les tests on le retourne dans la réponse
        return Ok(new { message = "Si un compte existe, un token de réinitialisation a été envoyé.", token });
    }


    /// <summary>
    /// Endpoint pour la réinitialisation du mot de passe d'un utilisateur.
    /// </summary>
    /// <param name="request">Les informations de l'utilisateur pour la réinitialisation du mot de passe.</param>
    /// <returns>Une réponse indiquant que le mot de passe a été réinitialisé avec succès.</returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            await _authService.ResetPasswordAsync(request);
            return Ok(new { message = "Mot de passe réinitialisé avec succès." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
