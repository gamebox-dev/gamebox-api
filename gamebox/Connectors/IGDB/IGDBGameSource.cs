using GameBox.IGDBResponse;
using GameBox.Models;
using System.Collections;
using System.Net.Http.Headers;

namespace GameBox.Connectors.IGDB
{
    public class IGDBGameSource : IGameSource
    {
        public async Task<ExternalGame>? SearchGames(string q)
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
                Content = new StringContent($"fields name,platforms,summary,cover;where name = \"{q}\";")
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

            Game? gameResp = null;
            using (var gameResponse = await client.SendAsync(gameRequest))
            {
                gameResponse.EnsureSuccessStatusCode();
                string gameRespString = await gameResponse.Content.ReadAsStringAsync();
                gameResp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Game>>(gameRespString)?.FirstOrDefault();
                if (gameResp == null)
                    return null;
            }

            var coverRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.igdb.com/v4/covers"),
                Content = new StringContent($"fields url;where game = {gameResp.id};")
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

            Cover? coverResp;
            using (var coverResponse = await client.SendAsync(coverRequest))
            {
                string coverRespString = await coverResponse.Content.ReadAsStringAsync();
                coverResp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cover>>(coverRespString)?.FirstOrDefault();
                if (coverResp == null)
                    return null;
            }


            string platformIDFilter = gameResp.platforms?.Count > 0 ? $"where id = ({string.Join(',', gameResp.platforms?.Select(x => x.ToString()))})" : string.Empty;
            var platformRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.igdb.com/v4/platforms"),
                Content = new StringContent($"fields abbreviation;{platformIDFilter};")
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
                    return null;
            }

            return new ExternalGame()
            {
                ExternalID = gameResp.id,
                Title = gameResp.name,
                Description = gameResp.summary,
                ImagePath = coverResp.url,
                Platforms = platformResp.Select(x =>
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
