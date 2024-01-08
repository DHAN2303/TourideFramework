using Touride.Framework.Abstractions.Ioc;

namespace Touride.Framework.Abstractions.Caching.CacheManagement
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class CacheAttribute : InterceptionAttribute
    {
        public string Key { get; set; }
        public string Region { get; set; }
        public string Policy { get; set; }
        public TimeSpan Expire { get; set; }
        public CacheExpirationTypeEnum ExpirationMode { get; set; }

        public ICacheKeySuffixSelector CacheKeySuffixSelector { get; set; }

        public CacheAttribute(string key, string policyName = Constants.DefaultPolicy, string region = Constants.DefaultRegion
            , int keySuffixArgumentIndex = -1, string keySuffixModelPropertyName = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
            Policy = policyName;
            Region = region;
            if (keySuffixArgumentIndex != -1)
            {
                CacheKeySuffixSelector = new ValueArgumentSuffixSelector(keySuffixArgumentIndex);
            }
            else if (!string.IsNullOrEmpty(keySuffixModelPropertyName))
            {
                CacheKeySuffixSelector = new ModelPropertySuffixSelector(keySuffixModelPropertyName);
            }
        }

        public CacheAttribute(string key, TimeSpan expire, CacheExpirationTypeEnum expirationMode, string region = Constants.DefaultRegion
            , int keySuffixArgumentIndex = -1, string keySuffixModelPropertyName = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
            Expire = expire;
            ExpirationMode = expirationMode;
            Region = region;
            if (keySuffixArgumentIndex != -1)
            {
                CacheKeySuffixSelector = new ValueArgumentSuffixSelector(keySuffixArgumentIndex);
            }
            else if (!string.IsNullOrEmpty(keySuffixModelPropertyName))
            {
                CacheKeySuffixSelector = new ModelPropertySuffixSelector(keySuffixModelPropertyName);
            }
        }
    }
}
