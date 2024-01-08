using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Touride.Framework.Client.Abstractions;
using Touride.Framework.Client.Extensions;

namespace Touride.Framework.Client.Providers
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public HttpClientProvider(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }

        //public async Task<T> ExecuteAsync<TService, T>(Func<TService, Task<T>> func)
        //{
        //    return await GetAsync<T>(func.Method.Name, func.Method.GetParameters().GetValue(0));
        //}

        /// <summary>
        /// For getting a single item from a web api uaing GET
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api get method, e.g. "products/1" to get a product with an id of 1</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The item requested</returns>
        public async Task<T> GetAsync<T>(string clientName, string apiUrl, string apiKey, string apiSecret, object parameter = null, CancellationToken cancellationToken = default)
        {
            if (parameter != null)
                apiUrl = apiUrl.AddQueryString(parameter);
            var client = await GetHttpClient(clientName, apiKey, apiSecret);
            var json = default(string);
            try
            {
                json = await client.GetStringAsync(apiUrl);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return default(T);
            }
            //var options = new JsonSerializerOptions
            //{
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //};

            var model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }

        public async Task<T> GetAsync<T>(string clientName, string apiUrl, object parameter = null, CancellationToken cancellationToken = default)
        {
            if (parameter != null)
                apiUrl = apiUrl.AddQueryString(parameter);
            var client = await GetHttpClient(clientName);
            var json = default(string);
            try
            {
                json = await client.GetStringAsync(apiUrl);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return default(T);
            }
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            // var model = JsonSerializer.Deserialize<T>(json, options);
            var model = JsonConvert.DeserializeObject<T>(json);

            return model;
        }

        public async Task<T> GetAsync<T>(string clientName, string apiUrl, string customToken, object parameter = null, CancellationToken token = default)
        {
            if (parameter != null)
                apiUrl = apiUrl.AddQueryString(parameter);
            var client = await GetHttpClient(clientName, customToken);
            var json = default(string);
            try
            {
                json = await client.GetStringAsync(apiUrl);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return default(T);
            }
            catch (Exception e)
            {
                var x = e.Message;
            }
            //var options = new JsonSerializerOptions
            //{
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //};

            var model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }
        /// <summary>
        /// For creating a new item over a web api using POST
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api post method, e.g. "products" to add products</param>
        /// <param name="postObject">The object to be created</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The item created</returns>
        public async Task<TResult> PostAsync<T, TResult>(string clientName, string apiUrl, T postObject, string apiKey, string apiSecret, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName, apiKey, apiSecret);
            var result = default(TResult);
            var payload = JsonConvert.SerializeObject(postObject);

            var response = await client.PostAsync(apiUrl, new StringContent(payload, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    result = JsonConvert.DeserializeObject<TResult>(x.Result);
                    //result = JsonSerializer.Deserialize<TResult>(x.Result, options);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        public async Task<TResult> PostAsync<T, TResult>(string clientName, string apiUrl, T postObject, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName);
            var result = default(TResult);
            var payload = JsonConvert.SerializeObject(postObject);

            var response = await client.PostAsync(apiUrl, new StringContent(payload, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    try
                    {
                        result = JsonConvert.DeserializeObject<TResult>(x.Result);

                    }
                    catch (Exception ex)
                    {
                        result = System.Text.Json.JsonSerializer.Deserialize<TResult>(x.Result, options);
                    }
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        public async Task<TResult> PostAsync<T, TResult>(string clientName, string apiUrl, T postObject, string customToken, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName, customToken);
            var result = default(TResult);
            var payload = JsonConvert.SerializeObject(postObject);

            var response = await client.PostAsync(apiUrl, new StringContent(payload, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };

                    result = JsonConvert.DeserializeObject<TResult>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }
        /// <summary>
        /// For updating an existing item over a web api using PUT
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api put method, e.g. "products/3" to update product with id of 3</param>
        /// <param name="putObject">The object to be edited</param>
        /// <param name="cancellationToken"></param>
        public async Task PutAsync<T>(string clientName, string apiUrl, T putObject, string apiKey, string apiSecret, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName, apiKey, apiSecret);
            var payload = JsonConvert.SerializeObject(putObject);
            //var payload = JsonSerializer.Serialize(putObject);
            var response = await client.PutAsync(apiUrl, new StringContent(payload, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
        }

        public async Task PutAsync<T>(string clientName, string apiUrl, T putObject, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName);
            var payload = JsonConvert.SerializeObject(putObject);
            var response = await client.PutAsync(apiUrl, new StringContent(payload, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
        }
        public async Task PutAsync<T>(string clientName, string apiUrl, T putObject, string customToken, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName, customToken);
            var payload = JsonConvert.SerializeObject(putObject);
            var response = await client.PutAsync(apiUrl, new StringContent(payload, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
        }

        public async Task<TResult> PutAsync<T, TResult>(string clientName, string apiUrl, T putObject, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName);
            var result = default(TResult);
            var payload = JsonConvert.SerializeObject(putObject);

            var response = await client.PutAsync(apiUrl, new StringContent(payload, Encoding.UTF8, "application/json"), cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    // result = JsonConvert.DeserializeObject<TResult>(x.Result);
                    result = System.Text.Json.JsonSerializer.Deserialize<TResult>(x.Result, options);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        /// <summary>
        /// For deleting an existing item over a web api using DELETE
        /// </summary>
        /// <param name="apiUrl">Added to the base address to make the full url of the 
        ///     api delete method, e.g. "products/3" to delete product with id of 3</param>
        /// <param name="cancellationToken"></param>
        public async Task DeleteAsync(string clientName, string apiUrl, string apiKey, string apiSecret, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName, apiKey, apiSecret);
            var response = await client.DeleteAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
        }
        public async Task DeleteAsync(string clientName, string apiUrl, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName);
            var response = await client.DeleteAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
        }
        public async Task DeleteAsync<T>(string clientName, string apiUrl, T putObject, string customToken, CancellationToken cancellationToken)
        {
            var client = await GetHttpClient(clientName, customToken);
            var response = await client.DeleteAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
        }

        private async Task<HttpClient> GetHttpClient(string clientName)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            var accessToken = await _tokenProvider.GetToken(TokenType.AccessToken);
            if (!string.IsNullOrEmpty(accessToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("dapr-app-id", clientName);
            return client;
        }

        private async Task<HttpClient> GetHttpClient(string clientName, string customToken)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            if (!string.IsNullOrEmpty(customToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", customToken);//.Add("AUTH_TOKEN", customToken); ;
            //client.DefaultRequestHeaders.Add("dapr-app-id", clientName);
            return client;
        }

        private async Task<HttpClient> GetHttpClient(string clientName, string apiKey, string apiSecret)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}");
            string value = System.Convert.ToBase64String(plainTextBytes);
            var client = _httpClientFactory.CreateClient(clientName);
            if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(apiSecret))
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + value);
            client.DefaultRequestHeaders.Add("dapr-app-id", clientName);
            return client;
        }

    }
}