using System.Net.Http.Headers;

namespace GameBox.Connectors
{
    /// <summary>
    /// Connector to an external resource.
    /// </summary>
    public class Connector
    {
        /// <summary>
        /// HTTP client used in internal requests.
        /// </summary>
        private HttpClient httpClient = new HttpClient();
        
        /// <summary>
        /// Make a POST request containing some body and headers.
        /// </summary>
        protected async Task<T?> PostRequest<T>(string uri, object body, Dictionary<string, string>? headers = null) where T: class
        {
            string? stringContent = null;
            MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("application/json");
            if (body is string) {
                stringContent = body as string;
            } else {
                stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                contentType = new MediaTypeHeaderValue("application/json");
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(stringContent ?? "")
                {
                    Headers =
                    {
                        ContentType = contentType
                    }
                }
            };

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> entry in headers)
                {
                    request.Headers.Add(entry.Key, entry.Value);
                }
            }

            T? content = null;
            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();
                content = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseString);
            }

            return content;
        }
    }
}
