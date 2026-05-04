using System.Text.Json.Serialization;

namespace GameDB.API.DTOs.Igdb;

/// <summary>
/// DTO que mapea exactamente la respuesta JSON de la API de IGDB para un juego.
/// Los nombres de propiedad coinciden con los campos de IGDB (snake_case).
/// </summary>
public class IgdbGameDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("rating")]
    public double? Rating { get; set; }

    [JsonPropertyName("rating_count")]
    public int? RatingCount { get; set; }

    [JsonPropertyName("first_release_date")]
    public long? FirstReleaseDate { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("cover")]
    public IgdbCoverDto? Cover { get; set; }

    [JsonPropertyName("genres")]
    public List<IgdbGenreDto>? Genres { get; set; }

    [JsonPropertyName("platforms")]
    public List<IgdbPlatformDto>? Platforms { get; set; }
}