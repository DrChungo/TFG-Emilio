using GameDB.API.DTOs.Igdb;
using GameDB.API.DTOs.Response;
using GameDB.API.Models;

namespace GameDB.API.Mappers;

/// <summary>
/// Responsable de transformar datos entre las distintas capas de la aplicación:
/// IgdbDto → Model (para persistir) y Model → ResponseDto (para enviar al frontend).
/// </summary>
public static class GameMapper
{
    // Constante para construir URLs de imágenes de IGDB
    private const string IgdbImageBaseUrl =
        "https://images.igdb.com/igdb/image/upload/t_cover_big/";

    /// <summary>
    /// Convierte un DTO de IGDB en una entidad Game lista para persistir en la BBDD.
    /// </summary>
    /// <param name="dto">Datos crudos recibidos de la API de IGDB.</param>
    /// <returns>Entidad <see cref="Game"/> mapeada.</returns>
    public static Game FromIgdbDto(IgdbGameDto dto)
    {
        return new Game
        {
            IgdbId = dto.Id,
            Name = dto.Name,
            Summary = dto.Summary,
            Rating = dto.Rating,
            RatingCount = dto.RatingCount,
            FirstReleaseDate = dto.FirstReleaseDate,
            Slug = dto.Slug,
            CoverUrl = BuildCoverUrl(dto.Cover?.ImageId),
            CachedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Convierte una entidad Game de la BBDD en un DTO de respuesta para el frontend.
    /// </summary>
    /// <param name="game">Entidad recuperada de la base de datos.</param>
    /// <returns>DTO listo para serializar y enviar como JSON.</returns>
    public static GameResponseDto ToResponseDto(Game game)
    {
        return new GameResponseDto
        {
            Id = game.Id,
            IgdbId = game.IgdbId,
            Name = game.Name,
            Summary = game.Summary,
            Rating = game.Rating,
            RatingCount = game.RatingCount,
            CoverUrl = game.CoverUrl,
            Slug = game.Slug,
            ReleaseYear = ConvertUnixToYear(game.FirstReleaseDate),
            Genres = game.GameGenres
                              .Select(gg => gg.Genre.Name)
                              .ToList(),
            Platforms = game.GamePlatforms
                              .Select(gp => gp.Platform.Name)
                              .ToList()
        };
    }

    /// <summary>
    /// Construye la URL completa de la imagen de portada a partir del image_id de IGDB.
    /// </summary>
    /// <param name="imageId">El image_id devuelto por IGDB.</param>
    /// <returns>URL completa de la imagen o null si no hay imagen.</returns>
    private static string? BuildCoverUrl(string? imageId)
    {
        if (string.IsNullOrWhiteSpace(imageId)) return null;
        return $"{IgdbImageBaseUrl}{imageId}.jpg";
    }

    /// <summary>
    /// Convierte un Unix timestamp a año calendario.
    /// </summary>
    /// <param name="unixTimestamp">Timestamp en segundos desde epoch.</param>
    /// <returns>Año como entero, o null si el timestamp es nulo.</returns>
    private static int? ConvertUnixToYear(long? unixTimestamp)
    {
        if (unixTimestamp is null) return null;
        return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp.Value).Year;
    }
}