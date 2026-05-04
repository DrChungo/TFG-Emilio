using System.Text.Json.Serialization;

namespace GameDB.API.DTOs.Igdb;

/// <summary>DTO para una plataforma devuelta por IGDB.</summary>
public class IgdbPlatformDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("abbreviation")]
    public string? Abbreviation { get; set; }
}