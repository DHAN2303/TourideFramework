using CacheManager.Core;

namespace Touride.Framework.Caching.Common.Configuration
{
    public static class CacheConfigurationBuilder
    {
        public static ConfigurationBuilderCachePart WithOptions(CachingOptions cachingOptions)
        {
            var cacheBuiler = new ConfigurationBuilder(cachingOptions.Name);
            if (cachingOptions.EnableCacheUpdateMode)
            {
                cacheBuiler.WithUpdateMode(CacheUpdateMode.Up);
            }
            else
            {
                cacheBuiler.WithUpdateMode(CacheUpdateMode.None);
            }
            return cacheBuiler;
        }
    }
}
