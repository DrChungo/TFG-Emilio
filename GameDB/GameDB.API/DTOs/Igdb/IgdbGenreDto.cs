using System.Text.Json.Serialization;

namespace GameDB.API.DTOs.Igdb;

/// <summary>DTO para un género devuelto por IGDB.</summary>
public class IgdbGenreDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}