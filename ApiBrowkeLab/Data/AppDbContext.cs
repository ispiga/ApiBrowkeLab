using Microsoft.EntityFrameworkCore;
using ApiBrowkeLab.Models;

namespace ApiBrowkeLab.Data;

/// <summary>
/// Contexto de Entity Framework Core para la base de datos
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar la entidad User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.PasswordHash)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Índice único para Email
            entity.HasIndex(e => e.Email)
                .IsUnique();
        });
    }
}
