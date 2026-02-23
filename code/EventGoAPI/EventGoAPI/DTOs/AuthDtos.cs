namespace EventGoAPI.DTOs;

// 3 DTOs pour les requêtes d'authentification et les réponses (immutable records)

public record RegisterRequest(
    string Email,
    string Username,
    string Password,
    string? FullName = null
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    string Token,
    DateTime Expiration,
    Guid UserId,
    string Email,
    string Username
);