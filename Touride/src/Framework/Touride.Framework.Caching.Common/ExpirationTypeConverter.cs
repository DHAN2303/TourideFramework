using CacheManager.Core;
using Touride.Framework.Abstractions.Caching;

namespace Touride.Framework.Caching.Common
{
    public static class ExpirationTypeConverter
    {
        public static ExpirationMode ToExpirationMode(this CacheExpirationTypeEnum cacheExpirationTypeEnum)
        {
            ExpirationMode expirationMode;
            switch (cacheExpirationTypeEnum)
            {
                case CacheExpirationTypeEnum.Absolute:
                    {
                        expirationMode = ExpirationMode.Absolute;
                        break;
                    }
                case CacheExpirationTypeEnum.Default:
                    {
                        expirationMode = ExpirationMode.Default;
                        break;
                    }
                case CacheExpirationTypeEnum.None:
                    {
                        expirationMode = ExpirationMode.None;
                        break;
                    }
                case CacheExpirationTypeEnum.Sliding:
                    {
                        expirationMode = ExpirationMode.Sliding;
                        break;
                    }
                default:
                    {
                        expirationMode = ExpirationMode.Default;
                        break;
                    }
            }
            return expirationMode;
        }
    }
}
