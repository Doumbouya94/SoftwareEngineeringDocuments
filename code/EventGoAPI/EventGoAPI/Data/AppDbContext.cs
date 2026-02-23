using EventGoAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventGoAPI.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Mappage de l'entité utilisateur vers la table MySQL
        builder.Entity<ApplicationUser>(e =>
        {
            e.ToTable("users");
            // Ef core genère automatiquement le GUID à l'insertion
            e.Property(u => u.Id).ValueGeneratedOnAdd();
            e.Property(u => u.FullName).HasColumnName("full_name");
            e.Property(u => u.ProfileImage).HasColumnName("profile_image");
            e.Property(u => u.IsGuest).HasColumnName("is_guest").HasDefaultValue(false);

            // MySQL utilise CURRENT_TIMESTAMP
            e.Property(u => u.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            e.Property(u => u.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");
        });
    }
}