using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Caching.Common.Configuration;

namespace Touride.Framework.Caching.Couchbase.Configuration
{
    public static class CouchbaseCachingServiceCollectionExtensions
    {
        public static IServiceCollection AddCouchbaseCache(this IServiceCollection services, IConfiguration configuration)
        {
            var couchbaseCachingOptions = new CouchbaseCachingOptions();
            configuration.Bind(CouchbaseCachingOptions.ConfigurationSection, couchbaseCachingOptions);
            var cacheConfiguration = GetCacheConfiguration(couchbaseCachingOptions);
            services.AddCache(configuration, cacheConfiguration, CouchbaseCachingOptions.ConfigurationSection);
            return services;
        }

        private static ICacheManagerConfiguration GetCacheConfiguration(CouchbaseCachingOptions cachingOptions)
        {
            return
                CacheConfigurationBuilder.WithOptions(cachingOptions)
                    .WithCouchbaseOptions(cachingOptions)
                    .Build();
        }
    }
}