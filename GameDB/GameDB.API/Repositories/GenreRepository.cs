using GameDB.API.Infrastructure;
using GameDB.API.Models;
using GameDB.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameDB.API.Repositories;

/// <summary>
/// Implementación concreta del repositorio de géneros.
/// </summary>
public class GenreRepository : IGenreRepository
{
    private readonly GameDbContext _context;

    /// <param name="context">Contexto de EF Core inyectado.</param>
    public GenreRepository(GameDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Genre?> GetByIgdbIdAsync(int igdbId)
    {
        return await _context.Genres
            .FirstOrDefaultAsync(g => g.IgdbId == igdbId);
    }

    /// <inheritdoc />
    public async Task AddAsync(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}