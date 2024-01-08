using Castle.DynamicProxy;
using Touride.Framework.Abstractions.Caching;
using Touride.Framework.Abstractions.Caching.CacheManagement;
using Touride.Framework.Abstractions.Helpers;
using Touride.Framework.IoC.Interception;

namespace Touride.Framework.Caching.Common.CacheManagement
{
    public class CacheInterceptor : IMethodInterceptor
    {
        private readonly ICacheManager<object> _cacheManager;

        public CacheInterceptor(ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public void Intercept(IInvocation invocation)
        {
            var cacheAttribute = GetCacheAttribute(invocation);
            if (cacheAttribute != null)
            {
                if (invocation.Method.IsAsync())
                {
                    PerformAsync(invocation, cacheAttribute);
                }
                else
                {
                    PerformSync(invocation, cacheAttribute);
                }
            }
            else
            {
                invocation.Proceed();

            }
            return;
        }

        private static CacheAttribute GetCacheAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            return (CacheAttribute)methodInfo.GetCustomAttributes(typeof(CacheAttribute), false).FirstOrDefault();
        }

        private void PerformAsync(IInvocation invocation, CacheAttribute cacheAttribute)
        {
            string key;
            var cachedItem = ReadFromCache(invocation, cacheAttribute, out key);
            if (cachedItem != null)
            {
                var returnTask = AsyncHelper.ConvertTaskType(Task.FromResult(cachedItem), invocation.Method.ReturnType.GenericTypeArguments[0]);
                invocation.ReturnValue = returnTask;
            }
            else
            {
                invocation.Proceed();
                AsyncHelper.CallAwaitTaskWithPostActionWithResultAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    (res) => { WriteToCahce(key, res, cacheAttribute); },
                    (ex) => { }
                );
            }
        }

        private void PerformSync(IInvocation invocation, CacheAttribute cacheAttribute)
        {
            string key;
            var cachedItem = ReadFromCache(invocation, cacheAttribute, out key);
            if (cachedItem != null)
            {
                invocation.ReturnValue = cachedItem;
            }
            else
            {
                invocation.Proceed();
                var returnValue = invocation.ReturnValue;
                WriteToCahce(key, returnValue, cacheAttribute);
            }
        }

        private object ReadFromCache(IInvocation invocation, CacheAttribute cacheAttribute, out string key)
        {
            key = GetKey(invocation, cacheAttribute);
            return _cacheManager.Get(key, cacheAttribute.Region);
        }

        private void WriteToCahce(string key, object value, CacheAttribute cacheAttribute)
        {
            if (value == null)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(cacheAttribute.Policy))
            {
                _cacheManager.Add(key, value, cacheAttribute.Policy, cacheAttribute.Region);
            }
            else
            {
                _cacheManager.Add(key, value, cacheAttribute.Expire, cacheAttribute.ExpirationMode, cacheAttribute.Region);
            }
        }

        private string GetKey(IInvocation invocation, CacheAttribute cacheAttribute)
        {
            if (cacheAttribute.CacheKeySuffixSelector != null)
            {
                return string.Concat(cacheAttribute.Key, cacheAttribute.CacheKeySuffixSelector.GetSuffix(invocation.Arguments));
            }
            else
            {
                return cacheAttribute.Key;
            }
        }
    }
}
