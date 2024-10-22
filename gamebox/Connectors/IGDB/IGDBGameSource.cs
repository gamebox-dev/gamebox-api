using GameBox.Connectors.IGDB.IGDBResponse;
using GameBox.Models;
using System.Collections;
using System.Net.Http.Headers;

namespace GameBox.Connectors.IGDB
{
    public class IGDBGameSource : IGameSource
    {
        public async Task<List<ExternalGame>> SearchGames(string q)
        {
            var client = new HttpClient();

            if (Environment.GetEnvironmentVariables() is not IDictionary variables)
                throw new Exception("Could not read environment variables");
            
            if(!variables.Contains("IGDB_CLIENT_ID"))
                throw new Exception("Environment variable IGDB_CLIENT_ID is missing, please make sure it is properly set in the registry or parent process");
            if (variables["IGDB_CLIENT_ID"] is not object clientIDObj)
                throw new Exception("Could not read environment variable IGDB_CLIENT_ID");
            if (clientIDObj is not string clientID)
                throw new Exception("Environment variable IGDB_CLIENT_ID is invalid");

            if (!variables.Contains("IGDB_CLIENT_SECRET"))
                throw new Exception("Environment variable IGDB_CLIENT_SECRET is missing, please make sure it is properly set in the registry or parent process");
            if (variables["IGDB_CLIENT_SECRET"] is not object clientSecretObj)
                throw new Exception("Could not read environment variable IGDB_CLIENT_SECRET");
            if (clientSecretObj is not string clientSecret)
                throw new Exception("Environment variable IGDB_CLIENT_SECRET is invalid");

            var tokenRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://id.twitch.tv/oauth2/token"),
                Content = new StringContent("{"+
                    $"\"client_id\": \"{clientID}\"," +
                    $"\"client_secret\": \"{clientSecret}\"," +
                    "\"grant_type\": \"client_credentials\"" + "}")
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };

            Token? tokenResp = null;
            using (var tokenResponse = await client.SendAsync(tokenRequest))
            {
                tokenResponse.EnsureSuccessStatusCode();
                string tokenRespString = await tokenResponse.Content.ReadAsStringAsync();
                tokenResp = Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(tokenRespString);
                if (tokenResp == null)
                    return null;
            }

            var gameRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.igdb.com/v4/games"),
                Content = new StringContent($"fields name,platforms,summary,cover;search \"{q}\";limit 10;")
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue ("text/plain")
                    }
                },
                Headers =
                {
                    { "Client-ID", $"{clientID}" },
                    { "Authorization", $"Bearer {tokenResp.access_token}" }
                },
            };

            List<Game?> gameResp;
            using (var gameResponse = await client.SendAsync(gameRequest))
            {
                gameResponse.EnsureSuccessStatusCode();
                string gameRespString = await gameResponse.Content.ReadAsStringAsync();
                gameResp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Game?>>(gameRespString) ?? new List<Game?>();
                if (gameResp.Count == 0)
                    return new List<ExternalGame>();
            }

            List<int> gameIDs = gameResp.Select(game => game?.id ?? -1).ToList();
            string gameIDsStr = string.Join(",", gameIDs.Select(id => id.ToString()));

            var coverRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.igdb.com/v4/covers"),
                Content = new StringContent($"fields url,game;where game = ({gameIDsStr});limit {gameIDs.Count};")
                {
                    Headers =
                    {
                        ContentType = MediaTypeHeaderValue.Parse ("text/plain")
                    }
                },
                Headers =
                {
                    { "Client-ID", clientID },
                    { "Authorization", $"Bearer {tokenResp.access_token}" }
                }
            };

            List<Cover?> coverResp;
            using (var coverResponse = await client.SendAsync(coverRequest))
            {
                string coverRespString = await coverResponse.Content.ReadAsStringAsync();
                coverResp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cover?>>(coverRespString) ?? new List<Cover?>();
                if (coverResp.Count == 0)
                    return new List<ExternalGame>();
            }

            string totalIDs = string.Join(",", gameResp.Select(game => string.Join(",", game.platforms.Select(platform => platform.ToString()))));
            int count = totalIDs.Split(",").Length;
            string platformIDFilter = totalIDs.Length > 0 ? $"where id = ({totalIDs})" : string.Empty;
            var platformRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.igdb.com/v4/platforms"),
                Content = new StringContent($"fields abbreviation;{platformIDFilter};limit {count};")
                {
                    Headers =
                    {
                        ContentType = MediaTypeHeaderValue.Parse("text/plain")
                    }
                },
                Headers =
                {
                    { "Client-ID", clientID },
                    { "Authorization", $"Bearer {tokenResp.access_token}" }
                }
            };

            List<Platform>? platformResp;
            using (var platformResponse = await client.SendAsync(platformRequest))
            {
                string platformRespString = await platformResponse.Content.ReadAsStringAsync();
                platformResp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Platform>>(platformRespString);
                if (platformResp == null)
                    return new List<ExternalGame>();
            }

            List<ExternalGame> externalGames = new List<ExternalGame>();
            foreach(Game? game in gameResp)
            {
                int extID = game.id;
                string title = game.name;
                string desc = game.summary;
                string imgPath = coverResp.Where(cover => cover.game == game.id).FirstOrDefault()?.url ?? string.Empty;
                List<ExternalPlatform> externalPlatforms = game.platforms.Select(plat =>
                {
                    Platform p = platformResp.Where(platform => platform.id == plat).FirstOrDefault() ?? new Platform();
                    return new ExternalPlatform
                    {
                        ID = p.id,
                        Name = p.abbreviation
                    };
                }).ToList();
                //foreach (int platID in game.platforms)
                //{
                //    foreach(Platform p in platformResp)
                //    {
                //        if(p.id == platID)
                //        {
                //            ExternalPlatform ep = new ExternalPlatform()
                //            {
                //                ID = p.id,
                //                Name = p.abbreviation
                //            };
                //            externalPlatforms.Add(ep);
                //            break;
                //        }
                //    }
                //}
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
