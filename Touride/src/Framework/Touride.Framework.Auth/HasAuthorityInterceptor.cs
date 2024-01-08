using Castle.DynamicProxy;
using Touride.Framework.Abstractions.Auth;
using Touride.Framework.IoC.Interception;

namespace Touride.Framework.Auth
{
    public class HasAuthorityInterceptor : IMethodInterceptor
    {

        private readonly List<TransantionInfoProvider> _transactionContext;
        public HasAuthorityInterceptor(List<TransantionInfoProvider> transactionContext)
        {
            _transactionContext = transactionContext;
        }
        public void Intercept(IInvocation invocation)
        {
            var authAttribute = GetAuthorityAttribute(invocation);
            if (authAttribute != null)
            {
                PerformAsync(invocation, authAttribute);
            }
            else
            {
                invocation.Proceed();

            }
            return;
        }

        private static HasAuthorityAttribute GetAuthorityAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            return (HasAuthorityAttribute)methodInfo.GetCustomAttributes(typeof(HasAuthorityAttribute), false).FirstOrDefault();
        }

        private void PerformAsync(IInvocation invocation, HasAuthorityAttribute cacheAttribute)
        {
            var authList = _transactionContext.ToList();
            bool isAuth = false;
            if (authList.Count > 0)
            {
                foreach (var item in cacheAttribute.Action)
                {
                    var transanctionInfo = authList.Find(p => p.Name == item);

                    if (transanctionInfo == null)
                    {
                        isAuth = false;
                        break;
                    }
                    isAuth = true;
                }
                //var hasAuth = authList.Where(p => p.Name == cacheAttribute.Action);
                if (isAuth)
                {
                    invocation.Proceed();
                }
                else
                {
                    throw new UnauthorizedAccessException("Unauthorized access");
                }
            }
            else
            {

                throw new UnauthorizedAccessException("Unauthorized access");
            }
        }


    }
}
