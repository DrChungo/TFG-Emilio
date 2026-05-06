using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GameDB.API.Config;
using GameDB.API.DTOs.Igdb;
using GameDB.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GameDB.API.Services;

/// <summary>
/// Servicio responsable de toda la comunicación con la API externa de IGDB.
/// Gestiona la autenticación OAuth2 de Twitch y construye las queries Apicalypse.
/// </summary>
public class IgdbService : IIgdbService
{
    private readonly HttpClient _httpClient;
    private readonly IgdbSettings _settings;
    private readonly ILogger<IgdbService> _logger;

    // Token OAuth2 cacheado en memoria para reutilizarlo hasta que expire
    private string? _cachedAccessToken;
    private DateTime _tokenExpiresAt = DateTime.MinValue;

    // Campos que pedimos a IGDB en cada consulta de juego
    private const string GameFields =
        "fields id,name,summary,rating,rating_count,first_release_date,slug,cover.image_id,genres.id,genres.name,platforms.id,platforms.name;";

    /// <param name="httpClientFactory">Factory para crear instancias de HttpClient.</param>
    /// <param name="settings">Configuración de IGDB inyectada desde appsettings.</param>
    /// <param name="logger">Logger para registrar errores y eventos.</param>
    public IgdbService(
        IHttpClientFactory httpClientFactory,
        IOptions<IgdbSettings> settings,
        ILogger<IgdbService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("IgdbClient");
        _settings = settings.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<List<IgdbGameDto>> SearchGamesAsync(string searchTerm, int limit = 10)
    {
        // Construimos la query en lenguaje Apicalypse de IGDB
        var query = $"""
            {GameFields}
            search "{searchTerm}";
            limit {limit};
            where rating != null;
            """;

        return await PostToIgdbAsync<List<IgdbGameDto>>("games", query)
               ?? new List<IgdbGameDto>();
    }

    /// <inheritdoc />
    public async Task<IgdbGameDto?> GetGameByIdAsync(int igdbId)
    {
        var query = $"""
            {GameFields}
            where id = {igdbId};
            limit 1;
            """;

        var results = await PostToIgdbAsync<List<IgdbGameDto>>("games", query);
        return results?.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<List<IgdbGameDto>> GetTopRatedGamesAsync(int limit = 20)
    {
        var query = $"""
            {GameFields}
            sort rating desc;
            where rating != null & rating_count > 100;
            limit {limit};
            """;

        return await PostToIgdbAsync<List<IgdbGameDto>>("games", query)
               ?? new List<IgdbGameDto>();
    }

    /// <summary>
    /// Método genérico que envía una query Apicalypse a un endpoint de IGDB
    /// y deserializa la respuesta al tipo indicado.
    /// </summary>
    /// <typeparam name="T">Tipo esperado en la respuesta JSON.</typeparam>
    /// <param name="endpoint">Endpoint de IGDB (ej: "games", "genres").</param>
    /// <param name="query">Query en formato Apicalypse.</param>
    /// <returns>Objeto deserializado o null si hay error.</returns>
    private async Task<T?> PostToIgdbAsync<T>(string endpoint, string query)
    {
        try
        {
            var token = await GetAccessTokenAsync();

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_settings.BaseUrl}/{endpoint}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("Client-ID", _settings.ClientId);
            request.Content = new StringContent(query, Encoding.UTF8, "text/plain");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error al consultar IGDB. Endpoint: {Endpoint}", endpoint);
            return default;
        }
    }

    /// <summary>
    /// Obtiene un token OAuth2 válido de Twitch. Reutiliza el token cacheado
    /// en memoria si aún no ha expirado, evitando llamadas innecesarias.
    /// </summary>
    /// <returns>Access token como string.</returns>
    /// <exception cref="InvalidOperationException">Si no se puede obtener el token.</exception>
    private async Task<string> GetAccessTokenAsync()
    {
        // Si el token aún es válido, lo reutilizamos
        if (_cachedAccessToken != null && DateTime.UtcNow < _tokenExpiresAt)
            return _cachedAccessToken;

        var tokenRequest = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id",     _settings.ClientId),
            new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
            new KeyValuePair<string, string>("grant_type",    "client_credentials")
        });

        var response = await _httpClient.PostAsync(_settings.AuthUrl, tokenRequest);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<TwitchTokenResponse>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (tokenData?.AccessToken is null)
            throw new InvalidOperationException(
                "No se pudo obtener el token de autenticación de Twitch/IGDB.");

        // Guardamos el token y calculamos cuándo expira (con 60s de margen)
        _cachedAccessToken = tokenData.AccessToken;
        _tokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenData.ExpiresIn - 60);

        _logger.LogInformation("Token de IGDB renovado. Expira en {Seconds}s",
            tokenData.ExpiresIn);

        return _cachedAccessToken;
    }
}

/// <summary>
/// DTO interno para deserializar la respuesta del token OAuth2 de Twitch.
/// </summary>
file class TwitchTokenResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}