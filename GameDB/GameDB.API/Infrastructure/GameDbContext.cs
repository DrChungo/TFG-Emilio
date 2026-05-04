using GameDB.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameDB.API.Infrastructure;

/// <summary>
/// Contexto principal de Entity Framework Core para GameDB.
/// Gestiona todas las entidades y sus relaciones en la base de datos MySQL.
/// </summary>
public class GameDbContext : DbContext
{
    /// <inheritdoc />
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

    // ── DbSets (tablas) ──────────────────────────────────────────
    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<GameGenre> GameGenres { get; set; }
    public DbSet<GamePlatform> GamePlatforms { get; set; }

    /// <summary>
    /// Configura las relaciones, claves compuestas y restricciones
    /// de las entidades mediante Fluent API.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── GameGenre: clave compuesta (muchos a muchos) ─────────
        modelBuilder.Entity<GameGenre>()
            .HasKey(gg => new { gg.GameId, gg.GenreId });

        modelBuilder.Entity<GameGenre>()
            .HasOne(gg => gg.Game)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GameId);

        modelBuilder.Entity<GameGenre>()
            .HasOne(gg => gg.Genre)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GenreId);

        // ── GamePlatform: clave compuesta (muchos a muchos) ──────
        modelBuilder.Entity<GamePlatform>()
            .HasKey(gp => new { gp.GameId, gp.PlatformId });

        modelBuilder.Entity<GamePlatform>()
            .HasOne(gp => gp.Game)
            .WithMany(g => g.GamePlatforms)
            .HasForeignKey(gp => gp.GameId);

        modelBuilder.Entity<GamePlatform>()
            .HasOne(gp => gp.Platform)
            .WithMany(p => p.GamePlatforms)
            .HasForeignKey(gp => gp.PlatformId);

        // ── Índices únicos para evitar duplicados de IGDB ────────
        modelBuilder.Entity<Game>()
            .HasIndex(g => g.IgdbId)
            .IsUnique();

        modelBuilder.Entity<Genre>()
            .HasIndex(g => g.IgdbId)
            .IsUnique();

        modelBuilder.Entity<Platform>()
            .HasIndex(p => p.IgdbId)
            .IsUnique();
    }
}