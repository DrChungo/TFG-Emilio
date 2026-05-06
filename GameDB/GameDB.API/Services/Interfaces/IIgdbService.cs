using GameDB.API.DTOs.Igdb;

namespace GameDB.API.Services.Interfaces;

/// <summary>
/// Contrato para el servicio de comunicación con la API externa de IGDB.
/// Abstrae la autenticación OAuth2 y las consultas a los endpoints de IGDB.
/// </summary>
public interface IIgdbService
{
    /// <summary>
    /// Busca juegos en IGDB por nombre.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda.</param>
    /// <param name="limit">Número máximo de resultados (máx. 500 en IGDB).</param>
    /// <returns>Lista de juegos crudos de IGDB.</returns>
    Task<List<IgdbGameDto>> SearchGamesAsync(string searchTerm, int limit = 10);

    /// <summary>
    /// Obtiene un juego específico de IGDB por su ID.
    /// </summary>
    /// <param name="igdbId">ID del juego en IGDB.</param>
    /// <returns>El juego encontrado o null si no existe.</returns>
    Task<IgdbGameDto?> GetGameByIdAsync(int igdbId);

    /// <summary>
    /// Obtiene los juegos más populares o mejor valorados de IGDB.
    /// </summary>
    /// <param name="limit">Número de juegos a recuperar.</param>
    /// <returns>Lista de juegos destacados.</returns>
    Task<List<IgdbGameDto>> GetTopRatedGamesAsync(int limit = 20);
}