namespace GameDB.API.Config;

/// <summary>
/// Configuración tipada para la integración con la API de IGDB.
/// Se inyecta automáticamente desde appsettings.json mediante IOptions.
/// </summary>
public class IgdbSettings
{
    /// <summary>Sección del appsettings que corresponde a esta clase.</summary>
    public const string SectionName = "IgdbSettings";

    /// <summary>URL base de la API de IGDB.</summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>URL para obtener el token OAuth2 de Twitch/IGDB.</summary>
    public string AuthUrl { get; set; } = string.Empty;

    /// <summary>Client ID de la aplicación registrada en Twitch Developer.</summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>Client Secret de la aplicación registrada en Twitch Developer.</summary>
    public string ClientSecret { get; set; } = string.Empty;
}