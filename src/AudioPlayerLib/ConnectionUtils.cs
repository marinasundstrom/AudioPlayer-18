using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AudioPlayer
{
    public static class ConnectionUtils
    {
        private static string getPlayerStatus;

        private static readonly string GetPlayerStatus = getPlayerStatus ?? (getPlayerStatus = JsonConvert.SerializeObject(new
        {
            GetPlayerStatus = new { }
        }));

        public static Task<bool> TestConnectionAsync(this IAudioPlayer audioPlayer)
        {
            if (audioPlayer is AudioPlayer ap)
            {
                return TestConnectionAsync(ap.baseUri, ap.username, ap.password);
            }

            throw new NotSupportedException("Not supported by the current implementation.");
        }

        public static async Task<bool> TestConnectionAsync(Uri baseUri, string username, string password)
        {
            const string relativeUri = "/local/player/api";
            var uri = new Uri(baseUri, relativeUri);

            var credCache = new CredentialCache
            {
                {
                    uri,
                    "Digest",
                    new NetworkCredential(username, password)
                }
            };

            var clientHandler = new HttpClientHandler
            {
                Credentials = credCache,
                PreAuthenticate = true
            };

            using (var client = new HttpClient(clientHandler)
            {
                BaseAddress = baseUri
            })
            {
                try
                {
                    var response = await client.PostAsync(relativeUri, new StringContent(GetPlayerStatus, Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(content);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
