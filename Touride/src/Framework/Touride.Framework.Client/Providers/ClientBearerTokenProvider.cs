using Newtonsoft.Json;
using Touride.Framework.Client.Abstractions;
using Touride.Framework.Client.Models;

namespace Touride.Framework.Client.Providers
{
    public class ClientBearerTokenProvider : IIdentityFlowProvider
    {
        private readonly string _url;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _audience;

        public ClientBearerTokenProvider(string url, string clientId, string clientSecret, string audience)
        {
            this._url = url;
            this._clientId = clientId;
            this._clientSecret = clientSecret;
            this._audience = audience;
        }

        public async Task<string> GetToken(TokenType type = TokenType.AccessToken)
        {
            //if (_cacheManager.Exists("ClientBearerTokenProvider")) return _cacheManager.Get("ClientBearerTokenProvider");

            var client = new HttpClient
            {
                BaseAddress = new Uri(_url)
            };

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("client_id", "integration_event_client"));
            nvc.Add(new KeyValuePair<string, string>("client_secret", "a6502307-90e7-4e4f-a9fb-8b160b291a50"));
            nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            var req = new HttpRequestMessage(HttpMethod.Post, "connect/token") { Content = new FormUrlEncodedContent(nvc) };
            var res = await client.SendAsync(req);
            var responseToken = await res.Content.ReadAsStringAsync();
            var tokenModel = JsonConvert.DeserializeObject<TokenModel>(responseToken);

            /*_cacheManager.AddOrUpdate("ClientBearerTokenProvider", tokenModel.AccessToken,
                expire: new TimeSpan(tokenModel.ExpiresIn), expirationMode: CacheExpirationTypeEnum.None);*/
            return tokenModel.AccessToken;
        }


    }
}