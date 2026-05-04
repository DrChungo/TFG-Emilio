namespace GameDB.API.Models;

/// <summary>
/// Tabla de unión entre Game y Platform (relación muchos a muchos).
/// </summary>
public class GamePlatform
{
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = null!;
}