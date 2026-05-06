using GameDB.API.Models;

namespace GameDB.API.Repositories.Interfaces;

/// <summary>
/// Contrato para el acceso a datos de la entidad Game.
/// Abstrae las operaciones de base de datos para facilitar
/// testing y cumplir con el principio de inversión de dependencias.
/// </summary>
public interface IGameRepository
{
    /// <summary>
    /// Busca un juego por su identificador de IGDB.
    /// </summary>
    /// <param name="igdbId">ID del juego en IGDB.</param>
    /// <returns>El juego encontrado con sus relaciones, o null si no existe.</returns>
    Task<Game?> GetByIgdbIdAsync(int igdbId);

    /// <summary>
    /// Obtiene un juego por su ID local incluyendo géneros y plataformas.
    /// </summary>
    /// <param name="id">ID local en nuestra base de datos.</param>
    /// <returns>El juego con relaciones cargadas, o null si no existe.</returns>
    Task<Game?> GetByIdAsync(int id);

    /// <summary>
    /// Devuelve una lista paginada de todos los juegos cacheados.
    /// </summary>
    /// <param name="page">Número de página (base 1).</param>
    /// <param name="pageSize">Cantidad de resultados por página.</param>
    /// <returns>Lista de juegos con sus relaciones.</returns>
    Task<List<Game>> GetAllAsync(int page, int pageSize);

    /// <summary>
    /// Busca juegos cuyo nombre contenga el término indicado.
    /// </summary>
    /// <param name="searchTerm">Texto a buscar en el nombre del juego.</param>
    /// <returns>Lista de juegos que coinciden con la búsqueda.</returns>
    Task<List<Game>> SearchByNameAsync(string searchTerm);

    /// <summary>
    /// Persiste un nuevo juego en la base de datos.
    /// </summary>
    /// <param name="game">Entidad Game a insertar.</param>
    Task AddAsync(Game game);

    /// <summary>
    /// Comprueba si un juego con ese IgdbId ya existe en la BBDD.
    /// </summary>
    /// <param name="igdbId">ID del juego en IGDB.</param>
    /// <returns>True si ya está cacheado, false si no.</returns>
    Task<bool> ExistsAsync(int igdbId);

    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos.
    /// </summary>
    Task SaveChangesAsync();
}