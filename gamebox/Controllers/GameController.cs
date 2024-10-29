using GameBox.Connectors;
using GameBox.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameBox.Controllers
{
    /// <summary>
    /// Game information service
    /// </summary>
    [Produces("application/json")]
    [Route("games/search")]
    public class GameController : Controller
    {
        IGameSource service;

        public GameController(IGameSource gameSource)
        {
            this.service = gameSource;
        }

        /// <summary>
        /// Retrieves a game from the IGDB by name
        /// </summary>
        /// <param name="q">The title of the game to search for</param>
        /// <returns></returns>
        [HttpGet(Name = "GetGame")]
        public Task<List<ExternalGame?>> Get(string q)
        {
            return service.SearchGames(q);
        }
    }
}
