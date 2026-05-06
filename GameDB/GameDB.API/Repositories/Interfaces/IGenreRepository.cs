using GameDB.API.Models;

namespace GameDB.API.Repositories.Interfaces;

/// <summary>
/// Contrato para el acceso a datos de la entidad Genre.
/// </summary>
public interface IGenreRepository
{
    /// <summary>
    /// Busca un género por su ID de IGDB.
    /// </summary>
    /// <param name="igdbId">ID del género en IGDB.</param>
    /// <returns>El género encontrado o null.</returns>
    Task<Genre?> GetByIgdbIdAsync(int igdbId);

    /// <summary>
    /// Persiste un nuevo género en la base de datos.
    /// </summary>
    /// <param name="genre">Entidad Genre a insertar.</param>
    Task AddAsync(Genre genre);

    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos.
    /// </summary>
    Task SaveChangesAsync();
}