using CacheManager.Core;
using Touride.Framework.Caching.Common;

namespace Touride.Framework.Caching.Couchbase.Configuration
{
    internal static class CacheConfigurationBuilderExtensions
    {
        internal static ConfigurationBuilderCachePart WithCouchbaseOptions(this ConfigurationBuilderCachePart configurationPart, CouchbaseCachingOptions cachingOptions)
        {
            var config = new ClientConfiguration
            {
                Servers = cachingOptions.Servers.ConvertAll(uri => new Uri(uri)),
                UseSsl = cachingOptions.UseSsl,
                BucketConfigs = new Dictionary<string, BucketConfiguration>
                {
                    {
                        cachingOptions.BucketName, new BucketConfiguration
                        {
                            BucketName = cachingOptions.BucketName,
                            Username = cachingOptions.Username,
                            Password = cachingOptions.Password
                        }
                    }
                }
            };

            ClusterHelper.Initialize(config);

            configurationPart
                .WithCouchbaseCacheHandle(cachingOptions.ConfigurationKey)
                .WithExpiration(cachingOptions.DefaultExpirtaionMode.ToExpirationMode(), cachingOptions.DefaultExpirationTimeout);

            return configurationPart;
        }
    }
}