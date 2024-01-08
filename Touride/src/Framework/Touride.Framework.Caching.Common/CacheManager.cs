using Touride.Framework.Abstractions.Caching;

namespace Touride.Framework.Caching.Common
{
    public class CacheManager<T> : ICacheManager<T>
    {
        private readonly CacheManager.Core.ICacheManager<T> _cacheManager;
        private readonly ICacheExpirationManager _cacheExpirationManager;

        public CacheManager(CacheManager.Core.ICacheManager<T> cacheManager, ICacheExpirationManager cacheExpirationManager)
        {
            _cacheManager = cacheManager;
            _cacheExpirationManager = cacheExpirationManager;
        }

        #region[ADD]

        public bool Add(ICacheItem<T> cacheItem)
        {
            var expirationSettings = _cacheExpirationManager.GetCacheExpirationSetting(cacheItem);
            return Add(cacheItem.Key,
                cacheItem.Value,
                expirationSettings.ExpirationTime,
                expirationSettings.ExpirationMode,
                GetRegionOrDefault(cacheItem));
        }
        public bool Add(string key, T value, string policyName = Constants.DefaultPolicy, string region = Constants.DefaultRegion)
        {
            var expirationSettings = _cacheExpirationManager.GetCacheExpirationSetting(key, policyName);
            return Add(key,
                value,
                expirationSettings.ExpirationTime,
                expirationSettings.ExpirationMode,
                GetRegionOrDefault(region));
        }
        public bool Add(string key, T value, TimeSpan expire, CacheExpirationTypeEnum expirationMode, string region = Constants.DefaultRegion)
        {
            var expireMode = expirationMode.ToExpirationMode();
            return _cacheManager.Add(new CacheManager.Core.CacheItem<T>(key, region, value, expireMode, expire));
        }

        #endregion[END_ADD]

        #region[ADD_OR_UPDATE]

        public void AddOrUpdate(ICacheItem<T> cacheItem)
        {
            var expirationSettings = _cacheExpirationManager.GetCacheExpirationSetting(cacheItem);
            AddOrUpdate(cacheItem.Key,
                cacheItem.Value,
                expirationSettings.ExpirationTime,
                expirationSettings.ExpirationMode,
                GetRegionOrDefault(cacheItem));
        }
        public void AddOrUpdate(string key, T value, string policyName = Constants.DefaultPolicy, string region = Constants.DefaultRegion)
        {
            var expirationSettings = _cacheExpirationManager.GetCacheExpirationSetting(key, policyName);
            AddOrUpdate(key,
                value,
                expirationSettings.ExpirationTime,
                expirationSettings.ExpirationMode,
                GetRegionOrDefault(region));
        }
        public void AddOrUpdate(string key, T value, TimeSpan expire, CacheExpirationTypeEnum expirationMode, string region = Constants.DefaultRegion)
        {
            var expireMode = expirationMode.ToExpirationMode();
            _cacheManager.Put(new CacheManager.Core.CacheItem<T>(key, region, value, expireMode, expire));
        }

        #endregion[END_ADD_OR_UPDATE]

        #region[GET]

        public T Get(ICacheItem<T> cacheItem)
        {
            return Get(cacheItem.Key, cacheItem.Region);
        }

        public T Get(string key, string region = Constants.DefaultRegion)
        {
            return _cacheManager.Get<T>(key, GetRegionOrDefault(region));
        }

        #endregion[END_GET]

        #region[EXISTS]

        public bool Exists(ICacheItem<T> cacheItem)
        {
            return Exists(cacheItem.Key, cacheItem.Region);
        }
        public bool Exists(string key, string region = Constants.DefaultRegion)
        {
            return _cacheManager.Exists(key, GetRegionOrDefault(region));
        }

        #endregion[END_EXISTS]

        #region[REMOVE]

        public bool Remove(ICacheItem<T> cacheItem)
        {
            return Remove(cacheItem.Key, cacheItem.Region);
        }

        public bool Remove(string key, string region = Constants.DefaultRegion)
        {
            return _cacheManager.Remove(key, GetRegionOrDefault(region));
        }

        #endregion[END_REMOVE]

        #region[CLEAR]

        public void Clear()
        {
            _cacheManager.Clear();
        }

        public void ClearRegion(ICacheItem<T> cacheItem)
        {
            _cacheManager.ClearRegion(cacheItem.Region);
        }
        public void ClearRegion(string region = Constants.DefaultRegion)
        {
            _cacheManager.ClearRegion(GetRegionOrDefault(region));
        }

        #endregion[END_CLEAR]

        private static string GetRegionOrDefault(ICacheItem<T> cacheItem)
        {
            //TODO: conflict
            return GetRegionOrDefault(cacheItem.Region);
        }
        private static string GetRegionOrDefault(string region)
        {
            return string.IsNullOrEmpty(region) ? Constants.DefaultRegion : region;
        }
    }
}
