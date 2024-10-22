using GameBox.Models;

namespace GameBox.Connectors
{
    /// <summary>
    /// Searches for games on external APIs
    /// </summary>
    public interface IGameSource
    {
        /// <summary>
        /// Searches for and returns a set of games based on a name search
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        Task<List<ExternalGame>> SearchGames(string q);
    }
}
