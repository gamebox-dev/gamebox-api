using GameBox.Connectors.IGDB.IGDBResponse;
using GameBox.Models;
using System.Collections;
using System.Net.Http.Headers;

namespace GameBox.Connectors.IGDB
{
    public class IGDBGameSource : Connector, IGameSource
    {
        /// <summary>
        /// File path to the file containing the authentication token.
        /// </summary>
        private const string AUTH_TOKEN_FILE = "IGDB_AUTH_TOKEN";

        /// <summary>
        /// IGDB authentication token.
        /// </summary>
        private string authToken;

        /// <summary>
        /// IGDB client ID.
        /// </summary>
        private string clientID;

        /// <summary>
        /// IGDB client secret.
        /// </summary>
        private string clientSecret;

        /// <summary>
        /// Constructs a new IGDB game source with the given client ID and secret.
        /// </summary>
        public IGDBGameSource(string clientID, string clientSecret)
        {
            this.clientID = clientID;
            this.clientSecret = clientSecret;

            var authFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GameBox",
                AUTH_TOKEN_FILE);
            Console.WriteLine($"Reading auth token from file: {authFilePath}");
            try
            {
                using StreamReader reader = new(authFilePath);
                authToken = reader.ReadToEnd();
                Console.WriteLine($"Got auth token: {authToken}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Unable to read auth token: {e.Message}");
                authToken = null;
            }
        }

        /// <summary>
        /// Sanitize/fix the URL of a cover coming from IGDB.
        /// </summary>
        public static string ConformCoverURL(string coverURL)
        {
            if (string.IsNullOrWhiteSpace(coverURL))
                return coverURL;

            if (coverURL.StartsWith('/'))
                coverURL = "https:" + coverURL;

            return coverURL.Replace("/t_thumb/", "/t_cover_big/");
        }

        /// <summary>
        /// Retrieves the auth token from the Twitch API, storing the results in a file.
        /// </summary>
        private async Task RetrieveToken()
        {
            Console.WriteLine("Retrieving auth token from IGDB API...");

            Token? token = await PostRequest<Token>(
                "https://id.twitch.tv/oauth2/token",
                new
                {
                    client_id = clientID,
                    client_secret = clientSecret,
                    grant_type = "client_credentials",
                }
            );
            if (token == null) {
                authToken = null;
                return;
            }

            var appData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GameBox");
            Directory.CreateDirectory(appData);
            var authFilePath = Path.Combine(appData, AUTH_TOKEN_FILE);
            using (StreamWriter outputFile = new StreamWriter(authFilePath))
            {
                await outputFile.WriteAsync(authToken);
            }

            authToken = token.access_token;
            Console.WriteLine($"Got auth token: {authToken}");
        }

        public async Task<List<ExternalGame>> SearchGames(string q)
        {
            if (authToken == null)
                await RetrieveToken();

            List<Game>? games = await PostRequest<List<Game>>(
                "https://api.igdb.com/v4/games",
                $"fields name,platforms,summary,cover;search \"{q}\";limit 50;",
                new Dictionary<string, string>
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {authToken}" }
                }
            );
            if (games?.Count == 0)
                return new List<ExternalGame>();

            string gameIDs = string.Join(",", games?.Select(game => game.id) ?? new List<int>());
            List<Cover>? covers = await PostRequest<List<Cover>>(
                "https://api.igdb.com/v4/covers",
                $"fields url,game;where game = ({gameIDs});",
                new Dictionary<string, string>
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {authToken}" }
                }
            );
            if (covers?.Count == 0)
                return new List<ExternalGame>();

            List<int> platformIDs = new List<int>();
            foreach (Game? game in games)
                platformIDs.AddRange(game.platforms);

            string uniquePlatformIDs = string.Join(",", platformIDs.Distinct().Select(p => p.ToString()));
            List<Platform>? platforms = await PostRequest<List<Platform>>(
                "https://api.igdb.com/v4/platforms",
                $"fields abbreviation;where id = ({uniquePlatformIDs});",
                new Dictionary<string, string>
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {authToken}" }
                }
            );
            if (platforms?.Count == 0)
                return new List<ExternalGame>();

            List<ExternalGame> externalGames = new List<ExternalGame>();
            foreach(Game? game in games)
            {
                int extID = game.id;
                string title = game.name;
                string desc = game.summary;
                string imgPath = ConformCoverURL(
                    covers?.Where(cover => cover.game == extID).FirstOrDefault()?.url
                    ?? string.Empty
                );
                List<ExternalPlatform> externalPlatforms = game.platforms.Select(plat =>
                {
                    Platform p = platforms?.Where(platform => platform.id == plat).FirstOrDefault() ?? new Platform();
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
