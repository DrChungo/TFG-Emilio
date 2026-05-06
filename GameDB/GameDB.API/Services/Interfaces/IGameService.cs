using GameDB.API.DTOs.Response;

namespace GameDB.API.Services.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con videojuegos.
/// Orquesta la caché progresiva: decide si servir desde BBDD o consultar IGDB.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Busca juegos por nombre. Consulta IGDB y cachea los resultados nuevos.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda introducido por el usuario.</param>
    /// <returns>Lista de juegos como DTOs de respuesta.</returns>
    Task<List<GameResponseDto>> SearchGamesAsync(string searchTerm);

    /// <summary>
    /// Obtiene un juego por su IgdbId. Si no está cacheado, lo obtiene de IGDB.
    /// </summary>
    /// <param name="igdbId">ID del juego en IGDB.</param>
    /// <returns>El juego como DTO de respuesta, o null si no se encuentra.</returns>
    Task<GameResponseDto?> GetGameByIgdbIdAsync(int igdbId);

    /// <summary>
    /// Obtiene los juegos mejor valorados. Cachea los que no estén en BBDD.
    /// </summary>
    /// <param name="page">Número de página.</param>
    /// <param name="pageSize">Resultados por página.</param>
    /// <returns>Lista paginada de juegos destacados.</returns>
    Task<List<GameResponseDto>> GetTopRatedGamesAsync(int page, int pageSize);
}