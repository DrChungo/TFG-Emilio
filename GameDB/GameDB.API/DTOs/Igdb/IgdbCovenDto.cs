using System.Text.Json.Serialization;

namespace GameDB.API.DTOs.Igdb;

/// <summary>
/// DTO para la imagen de portada devuelta por IGDB.
/// </summary>
public class IgdbCoverDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Image ID usado para construir la URL final de la imagen.
    /// Formato: https://images.igdb.com/igdb/image/upload/t_cover_big/{ImageId}.jpg
    /// </summary>
    [JsonPropertyName("image_id")]
    public string? ImageId { get; set; }
}