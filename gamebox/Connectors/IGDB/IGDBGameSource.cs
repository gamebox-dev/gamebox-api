using GameBox.Connectors.IGDB.IGDBResponse;
using GameBox.Models;
using GameBox.Utilities;
using System.Collections;
using System.Net.Http.Headers;

namespace GameBox.Connectors.IGDB
{
    public class IGDBGameSource : Connector, IGameSource
    {
        public async Task<List<ExternalGame>> SearchGames(string q)
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
                $"fields name,platforms,summary,cover;search \"{q}\";limit 10;",
                new Dictionary<string, string>
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {token.access_token}" }
                }
            );
            if (games == null || games.Count == 0)
                return null;

            string gameIDs = string.Join(",", games?.Select(game => game.id) ?? new List<int>());
            List<Cover>? covers = await PostRequest<List<Cover>>(
                "https://api.igdb.com/v4/covers",
                $"fields url,game;where game = ({gameIDs});",
                new Dictionary<string, string>
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {token.access_token}" }
                }
            );

            string totalIDs = string.Join(",", games.Select(game => string.Join(",", game.platforms.Select(platform => platform.ToString()))));
            int count = totalIDs.Split(",").Length;
            string platformIDFilter = totalIDs.Length > 0 ? $"where id = ({totalIDs});" : string.Empty;
            List<Platform>? platforms = await PostRequest<List<Platform>>(
                "https://api.igdb.com/v4/platforms",
                $"fields abbreviation;{platformIDFilter}",
                new Dictionary<string, string>
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {token.access_token}" }
                }
            );
            if (platforms == null)
                return null;

            List<ExternalGame> externalGames = new List<ExternalGame>();
            foreach(Game? game in games)
            {
                int extID = game.id;
                string title = game.name;
                string desc = game.summary;
                string imgPath = covers.Where(cover => cover.game == extID).FirstOrDefault()?.url ?? string.Empty;
                List<ExternalPlatform> externalPlatforms = game.platforms.Select(plat =>
                {
                    Platform p = platforms.Where(platform => platform.id == plat).FirstOrDefault() ?? new Platform();
                    return new ExternalPlatform
                    {
                        ID = p.id,
                        Name = p.abbreviation
                    };
                }).ToList();

                ExternalGame externalGame = new ExternalGame()
                {
                    ExternalID = extID,
                    Title = title,
                    Description = desc,
                    ImagePath = imgPath,
                    Platforms = externalPlatforms
                };
                externalGames.Add(externalGame);
            }

            return externalGames;
        }
    }
}
