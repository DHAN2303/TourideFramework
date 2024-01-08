using CacheManager.Core;
using Touride.Framework.Caching.Common;

namespace Touride.Framework.Caching.InMemory.Configuration
{
    internal static class CacheConfigurationBuilderExtensions
    {
        internal static ConfigurationBuilderCachePart WithInMemoryOptions(this ConfigurationBuilderCachePart configurationPart, InMemoryCachingOptions cachingOptions)
        {
            configurationPart.WithMicrosoftMemoryCacheHandle()
                .WithExpiration(cachingOptions.DefaultExpirtaionMode.ToExpirationMode(), cachingOptions.DefaultExpirationTimeout);
            if (cachingOptions.EnableJsonSerializer)
            {
                configurationPart.WithJsonSerializer();
            }
            return configurationPart;
        }
    }
}
