using GameDB.API.Models;

namespace GameDB.API.Repositories.Interfaces;

/// <summary>
/// Contrato para el acceso a datos de la entidad Platform.
/// </summary>
public interface IPlatformRepository
{
    /// <summary>
    /// Busca una plataforma por su ID de IGDB.
    /// </summary>
    /// <param name="igdbId">ID de la plataforma en IGDB.</param>
    /// <returns>La plataforma encontrada o null.</returns>
    Task<Platform?> GetByIgdbIdAsync(int igdbId);

    /// <summary>
    /// Persiste una nueva plataforma en la base de datos.
    /// </summary>
    /// <param name="platform">Entidad Platform a insertar.</param>
    Task AddAsync(Platform platform);

    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos.
    /// </summary>
    Task SaveChangesAsync();
}