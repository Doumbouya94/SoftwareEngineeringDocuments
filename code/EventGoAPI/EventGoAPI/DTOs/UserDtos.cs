namespace EventGoAPI.DTOs;

/// <summary>
/// DTO pour la requête de mise à jour du profil utilisateur.
/// </summary>
/// <param name="FullName"></param>
/// <param name="Age"></param>
/// <param name="ProfileImage"></param>
public record UpdateProfileRequest(
    string? FullName,
    int? Age,
    string? ProfileImage
);


/// <summary>
/// DTO pour la réponse du profil utilisateur, contenant les informations de base du profil d'un utilisateur.
/// </summary>
/// <param name="Id"></param>
/// <param name="Email"></param>
/// <param name="Username"></param>
/// <param name="FullName"></param>
/// <param name="Age"></param>
/// <param name="ProfileImage"></param>
/// <param name="IsGuest"></param>
/// <param name="CreatedAt"></param>
public record UserProfileResponse(
    Guid Id,
    string Email,
    string Username,
    string? FullName,
    int? Age,
    string? ProfileImage,
    bool IsGuest,
    DateTime CreatedAt
);