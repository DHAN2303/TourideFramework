using Touride.Framework.Abstractions.Ioc;

namespace Touride.Framework.Abstractions.Caching.CacheManagement
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ClearCacheAttribute : InterceptionAttribute
    {
        public string Key { get; set; }
        public string Region { get; set; }
        public ICacheKeySuffixSelector CacheKeySuffixSelector { get; set; }

        public ClearCacheAttribute(string key, string region = Constants.DefaultRegion
            , int keySuffixArgumentIndex = -1, string keySuffixModelPropertyName = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
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
