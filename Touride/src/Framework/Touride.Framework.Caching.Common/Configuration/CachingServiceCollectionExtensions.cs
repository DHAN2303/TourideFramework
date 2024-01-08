﻿using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Abstractions.Caching;

namespace Touride.Framework.Caching.Common.Configuration
{
    public static class CachingServiceCollectionExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration, ICacheManagerConfiguration cacheConfiguration, string configurationSection)
        {
            services.Configure<CachingOptions>(configuration.GetSection(configurationSection));
            services.AddSingleton(cacheConfiguration);
            services.AddSingleton<ICacheExpirationManager, CacheExpirationManager>();
            services.AddSingleton(typeof(CacheManager.Core.ICacheManager<>), typeof(BaseCacheManager<>));
            services.AddSingleton(typeof(Abstractions.Caching.ICacheManager<>), typeof(CacheManager<>));
            return services;
        }
    }
}
