using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AudioPlayer.Api
{
    public abstract class ApiBase
    {
        public string RelativeApiUri = null;
        public Uri BaseUri;
        private Uri Uri;
        public HttpClient client;

        protected void Initialize(Uri baseUri, NetworkCredential credentials)
        {
            BaseUri = baseUri;
            Uri = new Uri(baseUri, RelativeApiUri);

            var credCache = new CredentialCache
            {
                {
                    Uri,
                    "Digest",
                    credentials
                }
            };

            var clientHandler = new HttpClientHandler
            {
                Credentials = credCache,
                PreAuthenticate = true
            };

            client = new HttpClient(clientHandler)
            {
                BaseAddress = baseUri
            };
        }

        protected Task PostAsync(string request, CancellationToken cancellationToken = default)
        {
            return client.PostAsync(RelativeApiUri, new StringContent(request, Encoding.UTF8, "application/json"), cancellationToken);
        }

        protected async Task<T> PostAsync<T>(HttpClient client, string request, CancellationToken cancellationToken = default)
        {
            var response = await client.PostAsync(RelativeApiUri, new StringContent(request, Encoding.UTF8, "application/json"), cancellationToken);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        protected Task<T> PostAsync<T>(string request, CancellationToken cancellationToken = default) => PostAsync<T>(client, request, cancellationToken);
    }
}
