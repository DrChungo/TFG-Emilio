namespace GameDB.API.Config;

/// <summary>
/// Configuración de la estrategia de caché progresiva.
/// Determina cuánto tiempo se considera válido un registro en la BBDD local.
/// </summary>
public class CacheSettings
{
    public const string SectionName = "CacheSettings";

    /// <summary>
    /// Número de días tras los cuales un registro se considera obsoleto
    /// y debe ser refrescado desde IGDB.
    /// </summary>
    public int ExpirationDays { get; set; } = 7;
}