namespace GameDB.API.Models;

/// <summary>
/// Representa un género de videojuego (Acción, RPG, Estrategia, etc.).
/// </summary>
public class Genre
{
    /// <summary>Identificador único local.</summary>
    public int Id { get; set; }

    /// <summary>Identificador del género en IGDB.</summary>
    public int IgdbId { get; set; }

    /// <summary>Nombre del género.</summary>
    public string Name { get; set; } = string.Empty;

    // ── Relaciones ──────────────────────────────────────────────
    public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
}