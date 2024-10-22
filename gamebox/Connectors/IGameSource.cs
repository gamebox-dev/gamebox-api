using GameBox.Models;

namespace GameBox.Connectors
{
    /// <summary>
    /// Searches for games on external APIs
    /// </summary>
    public interface IGameSource
    {
        /// <summary>
        /// Searches for a game by exact name
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        Task<ExternalGame?> SearchGames(string q);
    }
}
