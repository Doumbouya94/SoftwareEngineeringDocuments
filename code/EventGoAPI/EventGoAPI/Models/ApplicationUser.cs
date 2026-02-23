using Microsoft.AspNetCore.Identity;

namespace EventGoAPI.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    // IdentityUser<Guid> provides: Id, Email, UserName, PasswordHash, etc.
    // Champs supplémentaire propres à l'application
    public string? FullName { get; set; }
    public int? Age { get; set; }
    public string? Phone { get; set; }
    public string? ProfileImage { get; set; }
    public bool IsGuest { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
