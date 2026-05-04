namespace GameDB.API.Models;

/// <summary>
/// Representa una plataforma de videojuegos (PS5, PC, Xbox, etc.).
/// </summary>
public class Platform
{
    /// <summary>Identificador único local.</summary>
    public int Id { get; set; }

    /// <summary>Identificador de la plataforma en IGDB.</summary>
    public int IgdbId { get; set; }

    /// <summary>Nombre de la plataforma.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Abreviatura de la plataforma (ej: "PS5", "PC").</summary>
    public string? Abbreviation { get; set; }

    // ── Relaciones ──────────────────────────────────────────────
    public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
}