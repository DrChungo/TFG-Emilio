using GameDB.API.Infrastructure;
using GameDB.API.Models;
using GameDB.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameDB.API.Repositories;

/// <summary>
/// Implementación concreta del repositorio de plataformas.
/// </summary>
public class PlatformRepository : IPlatformRepository
{
    private readonly GameDbContext _context;

    /// <param name="context">Contexto de EF Core inyectado.</param>
    public PlatformRepository(GameDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Platform?> GetByIgdbIdAsync(int igdbId)
    {
        return await _context.Platforms
            .FirstOrDefaultAsync(p => p.IgdbId == igdbId);
    }

    /// <inheritdoc />
    public async Task AddAsync(Platform platform)
    {
        await _context.Platforms.AddAsync(platform);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}