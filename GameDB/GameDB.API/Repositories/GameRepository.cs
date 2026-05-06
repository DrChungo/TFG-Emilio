using GameDB.API.Infrastructure;
using GameDB.API.Models;
using GameDB.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameDB.API.Repositories;

/// <summary>
/// Implementación concreta del repositorio de juegos.
/// Toda interacción con la tabla Games pasa por aquí.
/// </summary>
public class GameRepository : IGameRepository
{
    private readonly GameDbContext _context;

    /// <param name="context">Contexto de EF Core inyectado.</param>
    public GameRepository(GameDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Obtiene un juego por su ID de IGDB, incluyendo sus géneros y plataformas relacionados.
    /// </summary>
    /// <param name="igdbId"></param>
    /// <returns></returns>
    public async Task<Game?> GetByIgdbIdAsync(int igdbId)
    {
        return await _context.Games
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .FirstOrDefaultAsync(g => g.IgdbId == igdbId);
    }

    /// <summary>
    /// Obtiene un juego por su ID interno, incluyendo sus géneros y plataformas relacionados.
    /// </summary>
    /// <param name="id">ID interno del juego.</param>
    /// <returns>El juego encontrado o null.</returns>
    public async Task<Game?> GetByIdAsync(int id)
    {
        return await _context.Games
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <summary>
    /// Obtiene una lista paginada de juegos, ordenados por rating descendente, incluyendo sus géneros y plataformas relacionados.
    /// </summary>
    /// <param name="page">Número de página a obtener.</param>
    /// <param name="pageSize">Cantidad de juegos por página.</param>
    /// <returns>Lista de juegos paginada.</returns>
    public async Task<List<Game>> GetAllAsync(int page, int pageSize)
    {
        // Paginación: saltamos los registros de páginas anteriores
        return await _context.Games
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .OrderByDescending(g => g.Rating)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene una lista de juegos cuyo nombre contiene el término de búsqueda,
    /// ordenados por rating descendente, incluyendo sus géneros y plataformas relacionados.
    /// </summary>
    /// <param name="searchTerm"></param>
    /// <returns></returns>
    public async Task<List<Game>> SearchByNameAsync(string searchTerm)
    {
        return await _context.Games
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .Where(g => g.Name.Contains(searchTerm))
            .OrderByDescending(g => g.Rating)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene una lista de juegos que pertenecen a un género específico, 
    /// ordenados por rating descendente, incluyendo sus géneros y plataformas relacionados.
    /// </summary>
    /// <param name="game"></param>
    /// <returns></returns>
    public async Task AddAsync(Game game)
    {
        await _context.Games.AddAsync(game);
    }

    /// <summary>
    /// Observa si ya existe un juego con el mismo IgdbId en la base de datos, lo que indica que ya está cacheado.
    /// </summary>
    /// <param name="igdbId">ID de IGDB del juego.</param>
    /// <returns>True si el juego existe, false en caso contrario.</returns>
    public async Task<bool> ExistsAsync(int igdbId)
    {
        return await _context.Games.AnyAsync(g => g.IgdbId == igdbId);
    }

    /// <summary>
    /// Guarda los cambios realizados en el contexto de la base de datos.
    /// </summary>
    /// <returns></returns>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}