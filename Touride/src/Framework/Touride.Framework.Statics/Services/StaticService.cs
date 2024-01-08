using Touride.Framework.Abstractions.Caching;
using Touride.Framework.Client.Abstractions;
using Touride.Framework.Client.Extensions;

namespace Touride.Framework.Statics.Services
{
    public class StaticService<T> : IStaticService<T>
    {
        private readonly ICacheManager<List<T>> _cacheStaticList;
        private readonly ICacheManager<T> _cacheStatic;
        private readonly ICacheManager<string> _cacheDynamicResource;
        private readonly IHttpClientProvider _httpClientProvider;

        private const string KeyFormat = "{0}.{1}";
        public StaticService(
            ICacheManager<List<T>> cacheStaticList,
            ICacheManager<T> cacheStatic,
            ICacheManager<string> cacheDynamicResource,
            IHttpClientProvider httpClientProvider)
        {
            _cacheStaticList = cacheStaticList;
            _cacheStatic = cacheStatic;
            _cacheDynamicResource = cacheDynamicResource;
            _httpClientProvider = httpClientProvider;
        }

        public async Task<string> GetDynamicResourceAsync(string key, Guid? entityId, string languageIndex, int? entity2Id = null)
        {
            string result = string.Empty;

            var format = string.Format(KeyFormat, key, entity2Id.HasValue ? entity2Id.Value : entityId).Replace("I", "i").ToLowerInvariant();

            var splitKeys = format.Split('.');


            if (splitKeys.Length > 5)
            {
                result = _cacheDynamicResource.Get(format, region: languageIndex);

                if (string.IsNullOrEmpty(result))
                {
                    result = await _httpClientProvider.GetAsync<string>(
                         clientName: "LanguageApi",
                         apiUrl: $"api/static/getdynamicresource" + QueryStringExtensions.AddQueryStringOnlyParameters(new { key = key, entityid = entityId, languageIndex = languageIndex, entity2Id = entity2Id }));

                }
            }
            return result;
        }

        public async Task<T> GetAsync(Guid id)
        {
            var result = _cacheStatic.Get($"{id}", region: typeof(T).Name);

            if (result is null)
            {
                result = await _httpClientProvider.GetAsync<T>(
                        clientName: "LanguageApi",
                        apiUrl: $"api/static/getregion");
            }

            return result;
        }
    }
}
