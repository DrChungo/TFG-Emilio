using IGDB.Models;

namespace GameDB.API.Models
{
    /// <summary>
    /// Tabla de unión entre Game y Genre (relación muchos a muchos).
    /// </summary>
    public class GameGenre
    {
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
    }
}
