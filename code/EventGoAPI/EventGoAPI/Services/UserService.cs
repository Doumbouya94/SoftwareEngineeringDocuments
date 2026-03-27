using EventGoAPI.DTOs;
using EventGoAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace EventGoAPI.Services;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    /// <summary>
    /// Méthode pour récupérer le profil d'un utilisateur en fonction de son identifiant.
    /// </summary>
    /// <param name="userId">L'identifiant de l'utilisateur.</param>
    /// <returns>Le profil de l'utilisateur.</returns>
    /// <exception cref="Exception">Lève une exception si l'utilisateur n'est pas trouvé.</exception>
    public async Task<UserProfileResponse> GetProfileAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new Exception("Utilisateur n'a pas été trouvé");
        }

        return MapToResponse(user);
    }

    /// <summary>
    /// Méthode pour mettre à jour le profil d'un utilisateur.
    /// </summary>
    /// <param name="userId">L'identifiant de l'utilisateur.</param>
    /// <param name="request">Les informations de mise à jour du profil.</param>
    /// <returns>Le profil mis à jour de l'utilisateur.</returns>
    /// <exception cref="Exception">Lève une exception si l'utilisateur n'est pas trouvé ou si la mise à jour échoue.</exception>
    public async Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new Exception("Utilisateur n'a pas été trouvé");
        }
        user.FullName = request.FullName ?? user.FullName;
        user.Age = request.Age ?? user.Age;
        user.ProfileImage = request.ProfileImage ?? user.ProfileImage;
        user.UpdatedAt = DateTime.UtcNow;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
        }
        return MapToResponse(user);
    }


    /// <summary>
    /// Méthode privée pour mapper un objet ApplicationUser vers un UserProfileResponse,
    /// en conservant uniquement les informations pertinentes pour le profil utilisateur.
    /// </summary>
    /// <param name="user">L'utilisateur à mapper.</param>
    /// <returns>Le profil de l'utilisateur.</returns>
    private static UserProfileResponse MapToResponse(ApplicationUser user) => 
        new(user.Id, user.Email!, user.UserName!, user.FullName, user.Age, user.ProfileImage, user.IsGuest, user.CreatedAt);
}
