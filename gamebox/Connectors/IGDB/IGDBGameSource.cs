using GameBox.Connectors.IGDB.IGDBResponse;
using GameBox.Models;
using GameBox.Utilities;
using System.Collections;
using System.Net.Http.Headers;

namespace GameBox.Connectors.IGDB
{
    public class IGDBGameSource : Connector, IGameSource
    {
        public async Task<ExternalGame>? SearchGames(string q)
        {
            var client = new HttpClient();
            var clientID = EnvironmentUtility.GetVariable("IGDB_CLIENT_ID");
            var clientSecret = EnvironmentUtility.GetVariable("IGDB_CLIENT_SECRET");

            Token? token = await PostRequest<Token>(
                "https://id.twitch.tv/oauth2/token",
                new
                {
                    client_id = clientID,
                    client_secret = clientSecret,
                    grant_type = "client_credentials",
                }
            );
            if (token == null)
                return null;

            List<Game>? games = await PostRequest<List<Game>>(
                "https://api.igdb.com/v4/games",
                $"fields name,platforms,summary,cover;where name = \"{q}\";",
                new Dictionary<string, string>
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {token.access_token}" }
                }
            );
            Game? game = games?.FirstOrDefault();
            if (game == null)
                return null;

            List<Cover>? covers = await PostRequest<List<Cover>>(
                "https://api.igdb.com/v4/covers",
                $"fields url;where game = {game.id};",
                new Dictionary<string, string>
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {token.access_token}" }
                }
            );
            Cover? cover = covers?.FirstOrDefault();

            string platformIDFilter = game.platforms?.Count > 0 ? $"where id = ({string.Join(',', game.platforms.Select(x => x.ToString()))})" : string.Empty;
            List<Platform>? platforms = await PostRequest<List<Platform>>(
                "https://api.igdb.com/v4/platforms",
                $"fields abbreviation;{platformIDFilter};",
                new Dictionary<string, string>
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {token.access_token}" }
                }
            );
            if (platforms == null)
                return null;

            return new ExternalGame()
            {
                ExternalID = game.id,
                Title = game.name,
                Description = game.summary,
                ImagePath = cover?.url,
                Platforms = platforms.Select(x =>
                {
                    return new ExternalPlatform()
                    {
                        ID = x.id,
                        Name = x.abbreviation
                    };
                }).ToList()
            };
        }
    }
}
