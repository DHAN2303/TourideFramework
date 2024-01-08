using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Caching.Common.Configuration;

namespace Touride.Framework.Caching.Redis.Configuration
{
    public static class RedisCachingServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCachingOptions = new RedisCachingOptions();
            configuration.Bind(RedisCachingOptions.ConfigurationSection, redisCachingOptions);

            var cacheConfiguration = GetCacheConfiguration(redisCachingOptions);
            services.AddCache(configuration, cacheConfiguration, RedisCachingOptions.ConfigurationSection);
            return services;
        }

        private static ICacheManagerConfiguration GetCacheConfiguration(RedisCachingOptions cachingOptions)
        {
            return
                 CacheConfigurationBuilder.WithOptions(cachingOptions)
                .WithRedisOptions(cachingOptions)
                .Build();
        }
    }
}
