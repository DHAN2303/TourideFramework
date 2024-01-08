using CacheManager.Core;
using Castle.DynamicProxy;
using Touride.Framework.Abstractions.Caching.CacheManagement;
using Touride.Framework.Abstractions.Helpers;
using Touride.Framework.IoC.Interception;

namespace Touride.Framework.Caching.Common.CacheManagement
{
    public class ClearCacheInterceptor : IMethodInterceptor
    {
        private readonly ICacheManager<object> _cacheManager;

        public ClearCacheInterceptor(ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public void Intercept(IInvocation invocation)
        {
            var clearCacheAttribute = GetClearCacheAttribute(invocation);
            if (clearCacheAttribute != null)
            {
                if (invocation.Method.IsAsync())
                {
                    PerformAsync(invocation, clearCacheAttribute);
                }
                else
                {
                    PerformSync(invocation, clearCacheAttribute);
                }
            }
            else
            {
                invocation.Proceed();

            }
            return;
        }

        private static ClearCacheAttribute GetClearCacheAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            return (ClearCacheAttribute)methodInfo.GetCustomAttributes(typeof(ClearCacheAttribute), false).FirstOrDefault();
        }

        private void PerformAsync(IInvocation invocation, ClearCacheAttribute clearCacheAttribute)
        {

            invocation.Proceed();
            AsyncHelper.CallAwaitTaskWithPostActionWithResultAndFinallyAndGetResult(
                invocation.Method.ReturnType.GenericTypeArguments[0],
                invocation.ReturnValue,
                (res) => { ClearCache(invocation, clearCacheAttribute); },
                (ex) => { }
            );
        }

        private void PerformSync(IInvocation invocation, ClearCacheAttribute clearCacheAttribute)
        {
            invocation.Proceed();
            ClearCache(invocation, clearCacheAttribute);
        }

        private void ClearCache(IInvocation invocation, ClearCacheAttribute clearCacheAttribute)
        {
            var key = GetKey(invocation, clearCacheAttribute);
            _cacheManager.Remove(key, clearCacheAttribute.Region);
        }

        private string GetKey(IInvocation invocation, ClearCacheAttribute clearCacheAttribute)
        {
            if (clearCacheAttribute.CacheKeySuffixSelector != null)
            {
                return string.Concat(clearCacheAttribute.Key, clearCacheAttribute.CacheKeySuffixSelector.GetSuffix(invocation.Arguments));
            }
            else
            {
                return clearCacheAttribute.Key;
            }
        }
    }
}
