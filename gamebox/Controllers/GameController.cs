using GameBox.IGDBResponse;
using GameBox.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace GameBox.Controllers
{
    /// <summary>
    /// Game information service
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    public class GameController : Controller
    {
        /// <summary>
        /// Retrieves a game from the IGDB by name
        /// </summary>
        /// <param name="gameTitle">The title of the game to search for</param>
        /// <returns></returns>
        [HttpGet(Name = "games/search")]
        public async Task<GameBox.Models.Game?> Get(string gameTitle)
        {
            HttpResponseMessage? tokenResponse = null;
            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new StringContent("{" + Environment.NewLine + 
                    "\"client_id\": \"gkerplqbddwqjd5s253q471ztu2pth\"," + Environment.NewLine +
                    "\"client_secret\": \"lsbgel1hnihcwnwh3a1y6rw9zmqvup\"," + Environment.NewLine + 
                    "\"grant_type\": \"client_credentials\"" + Environment.NewLine +
                "}");
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                tokenResponse = await client.PostAsync("https://id.twitch.tv/oauth2/token", content);
            }

            if(tokenResponse == null)
                return null;

            string tokenRespString = await tokenResponse.Content.ReadAsStringAsync();
            Token? tokenResp = Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(tokenRespString);

            if (tokenResp == null || string.IsNullOrWhiteSpace(tokenResp.access_token))
                return null;

            HttpResponseMessage? gameResponse = null;
            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new StringContent($"fields name,external_games,summary,cover;\r\nwhere name = \"{gameTitle}\";");
                content.Headers.Add("Client-ID", "gkerplqbddwqjd5s253q471ztu2pth");
                content.Headers.Add("Authorization", "Bearer " + tokenResp.access_token);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                gameResponse = await client.PostAsync("https://api.igdb.com/v4/games", content);
            }

            if (gameResponse == null)
                return null;

            string gameRespString = await gameResponse.Content.ReadAsStringAsync();
            GameBox.IGDBResponse.Game? gameResp = Newtonsoft.Json.JsonConvert.DeserializeObject<GameBox.IGDBResponse.Game>(gameRespString);

            if (gameResp == null)
                return null;

            HttpResponseMessage? coverResponse = null;
            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new StringContent($"fields url;\r\nwhere game = {gameResp.id}");
                content.Headers.Add("Client-ID", "gkerplqbddwqjd5s253q471ztu2pth");
                content.Headers.Add("Authorization", "Bearer " + tokenResp.access_token);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                coverResponse = await client.PostAsync("https://api.igdb.com/v4/covers", content);
            }

            if(coverResponse == null)
                return null;

            string coverRespString = await coverResponse.Content.ReadAsStringAsync();
            GameBox.IGDBResponse.Cover? coverResp = Newtonsoft.Json.JsonConvert.DeserializeObject<GameBox.IGDBResponse.Cover>(coverRespString);

            if(coverResp == null)
                return null;

            return new GameBox.Models.Game()
            {
                External_ID = gameResp.external_games,
                Title = gameResp.name,
                Description = gameResp.summary,
                ImagePath = coverResp.url
            };
        }
    }
}
