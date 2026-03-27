using EventGoAPI.DTOs;
using EventGoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventGoAPI.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Endpoint pour récupérer le profil de l'utilisateur actuellement connecté.
    /// </summary>
    /// <returns>Le profil de l'utilisateur si disponible, sinon une réponse Unauthorized.</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        try 
        {
            return Ok(await _userService.GetProfileAsync(userId.Value));
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }


    /// <summary>
    /// Endpoint pour mettre à jour le profil de l'utilisateur actuellement connecté.
    /// </summary>
    /// <param name="request">Les informations de profil à mettre à jour.</param>
    /// <returns>Le profil mis à jour de l'utilisateur si disponible, sinon une réponse Unauthorized.</returns>
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        try
        {
            return Ok(await _userService.UpdateProfileAsync(userId.Value, request));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    /// <summary>
    /// Prend l'identifiant de l'utilisateur actuellement connecté à partir des revendications du token JWT.
    /// Cette méthode recherche d'abord la revendication standard "NameIdentifier" (utilisée par ASP.NET Identity),
    /// puis la convertit en Guid. Si la revendication n'est pas trouvée ou si la conversion échoue, elle retourne null.
    /// </summary>
    /// <returns>L'identifiant de l'utilisateur si disponible, sinon null.</returns>
    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        return Guid.TryParse(claim?.Value, out var id) ? id : null;
    }
}
