namespace Touride.Framework.Abstractions.Caching.CacheManagement
{
    public interface ICacheKeySuffixSelector
    {
        public string GetSuffix(object[] arguments);
    }
}
