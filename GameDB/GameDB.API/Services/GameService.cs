using GameDB.API.DTOs.Response;
using GameDB.API.Mappers;
using GameDB.API.Models;
using GameDB.API.Repositories.Interfaces;
using GameDB.API.Services.Interfaces;

namespace GameDB.API.Services;

/// <summary>
/// Servicio principal de lógica de negocio para videojuegos.
/// Implementa la estrategia de caché progresiva:
/// primero busca en BBDD local, si no existe consulta IGDB y lo persiste.
/// </summary>
public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IPlatformRepository _platformRepository;
    private readonly IIgdbService _igdbService;
    private readonly ILogger<GameService> _logger;

    public GameService(
        IGameRepository gameRepository,
        IGenreRepository genreRepository,
        IPlatformRepository platformRepository,
        IIgdbService igdbService,
        ILogger<GameService> logger)
    {
        _gameRepository = gameRepository;
        _genreRepository = genreRepository;
        _platformRepository = platformRepository;
        _igdbService = igdbService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<List<GameResponseDto>> SearchGamesAsync(string searchTerm)
    {
        // 1. Buscamos primero en nuestra caché local
        var cachedGames = await _gameRepository.SearchByNameAsync(searchTerm);

        if (cachedGames.Any())
        {
            _logger.LogInformation(
                "Cache HIT: búsqueda '{Term}' resuelta desde BBDD local.", searchTerm);
            return cachedGames.Select(GameMapper.ToResponseDto).ToList();
        }

        // 2. No hay resultados locales → consultamos IGDB
        _logger.LogInformation(
            "Cache MISS: consultando IGDB para '{Term}'.", searchTerm);

        var igdbGames = await _igdbService.SearchGamesAsync(searchTerm);

        // 3. Persistimos cada juego nuevo en la BBDD (caché progresiva)
        foreach (var igdbGame in igdbGames)
        {
            if (!await _gameRepository.ExistsAsync(igdbGame.Id))
                await CacheGameAsync(igdbGame);
        }

        // 4. Devolvemos los resultados ya cacheados con relaciones cargadas
        var freshResults = await _gameRepository.SearchByNameAsync(searchTerm);
        return freshResults.Select(GameMapper.ToResponseDto).ToList();
    }

    /// <inheritdoc />
    public async Task<GameResponseDto?> GetGameByIgdbIdAsync(int igdbId)
    {
        // 1. Comprobamos si ya está en caché
        var cached = await _gameRepository.GetByIgdbIdAsync(igdbId);
        if (cached is not null)
        {
            _logger.LogInformation(
                "Cache HIT: juego IgdbId={Id} servido desde BBDD.", igdbId);
            return GameMapper.ToResponseDto(cached);
        }

        // 2. No está → lo pedimos a IGDB
        _logger.LogInformation(
            "Cache MISS: obteniendo juego IgdbId={Id} de IGDB.", igdbId);

        var igdbGame = await _igdbService.GetGameByIdAsync(igdbId);
        if (igdbGame is null) return null;

        // 3. Lo persistimos y devolvemos
        await CacheGameAsync(igdbGame);

        var saved = await _gameRepository.GetByIgdbIdAsync(igdbId);
        return saved is null ? null : GameMapper.ToResponseDto(saved);
    }

    /// <inheritdoc />
    public async Task<List<GameResponseDto>> GetTopRatedGamesAsync(int page, int pageSize)
    {
        // 1. Intentamos servir desde caché local paginada
        var cached = await _gameRepository.GetAllAsync(page, pageSize);

        if (cached.Any())
        {
            _logger.LogInformation(
                "Cache HIT: top rated página {Page} servida desde BBDD.", page);
            return cached.Select(GameMapper.ToResponseDto).ToList();
        }

        // 2. Si la caché está vacía, poblamos desde IGDB
        _logger.LogInformation("Cache MISS: obteniendo top rated de IGDB.");

        var igdbGames = await _igdbService.GetTopRatedGamesAsync(pageSize);

        foreach (var igdbGame in igdbGames)
        {
            if (!await _gameRepository.ExistsAsync(igdbGame.Id))
                await CacheGameAsync(igdbGame);
        }

        var freshResults = await _gameRepository.GetAllAsync(page, pageSize);
        return freshResults.Select(GameMapper.ToResponseDto).ToList();
    }

    /// <summary>
    /// Persiste un juego de IGDB en la base de datos local junto con sus
    /// géneros y plataformas, creando las relaciones muchos-a-muchos.
    /// </summary>
    /// <param name="igdbGame">DTO del juego recibido de IGDB.</param>
    private async Task CacheGameAsync(DTOs.Igdb.IgdbGameDto igdbGame)
    {
        try
        {
            var game = GameMapper.FromIgdbDto(igdbGame);

            //  Géneros 
            if (igdbGame.Genres is not null)
            {
                foreach (var igdbGenre in igdbGame.Genres)
                {
                    // Reutilizamos el género si ya existe en BBDD
                    var genre = await _genreRepository.GetByIgdbIdAsync(igdbGenre.Id)
                                ?? new Genre { IgdbId = igdbGenre.Id, Name = igdbGenre.Name };

                    if (genre.Id == 0)
                        await _genreRepository.AddAsync(genre);

                    game.GameGenres.Add(new GameGenre
                    {
                        Genre = genre
                    });
                }
            }

            // Plataformas 
            if (igdbGame.Platforms is not null)
            {
                foreach (var igdbPlatform in igdbGame.Platforms)
                {
                    var platform = await _platformRepository.GetByIgdbIdAsync(igdbPlatform.Id)
                                   ?? new Platform { IgdbId = igdbPlatform.Id, Name = igdbPlatform.Name };

                    if (platform.Id == 0)
                        await _platformRepository.AddAsync(platform);

                    game.GamePlatforms.Add(new GamePlatform
                    {
                        Platform = platform
                    });
                }
            }

            await _gameRepository.AddAsync(game);
            await _gameRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Juego cacheado: '{Name}' (IgdbId={Id})", game.Name, game.IgdbId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error al cachear el juego IgdbId={Id}", igdbGame.Id);
        }
    }
}