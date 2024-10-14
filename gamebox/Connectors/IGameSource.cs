using GameBox.Models;

namespace GameBox.Connectors
{
    public interface IGameSource
    {
        Task<ExternalGame>? SearchGames(string q);
    }
}
