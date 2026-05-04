namespace GameDB.API.Models;

/// <summary>
/// Representa un videojuego almacenado en la base de datos local.
/// Esta entidad actúa como caché progresiva de los datos obtenidos de IGDB.
/// </summary>
public class Game
{
    /// <summary>Identificador único en nuestra base de datos local.</summary>
    public int Id { get; set; }

    /// <summary>Identificador original del juego en la API de IGDB.</summary>
    public int IgdbId { get; set; }

    /// <summary>Nombre del videojuego.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Descripción o sinopsis del juego.</summary>
    public string? Summary { get; set; }

    /// <summary>Puntuación media de la crítica (0-100).</summary>
    public double? Rating { get; set; }

    /// <summary>Número total de votos que componen el rating.</summary>
    public int? RatingCount { get; set; }

    /// <summary>Fecha de lanzamiento en formato Unix timestamp.</summary>
    public long? FirstReleaseDate { get; set; }

    /// <summary>URL de la imagen de portada obtenida de IGDB.</summary>
    public string? CoverUrl { get; set; }

    /// <summary>Slug único del juego (usado en URLs amigables).</summary>
    public string? Slug { get; set; }

    /// <summary>Fecha en que este registro fue guardado en nuestra BBDD.</summary>
    public DateTime CachedAt { get; set; } = DateTime.UtcNow;

    // ── Relaciones ──────────────────────────────────────────────
    public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
    public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
}