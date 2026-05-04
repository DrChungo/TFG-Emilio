namespace GameDB.API.DTOs.Response;

/// <summary>
/// DTO de respuesta que el backend envía al frontend.
/// Contiene solo los campos necesarios para la UI, sin exponer
/// detalles internos de la base de datos.
/// </summary>
public class GameResponseDto
{
    public int Id { get; set; }
    public int IgdbId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public double? Rating { get; set; }
    public int? RatingCount { get; set; }
    public string? CoverUrl { get; set; }
    public string? Slug { get; set; }

    /// <summary>Año de lanzamiento calculado desde el Unix timestamp.</summary>
    public int? ReleaseYear { get; set; }

    public List<string> Genres { get; set; } = new();
    public List<string> Platforms { get; set; } = new();
}