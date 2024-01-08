using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Touride.Framework.Api.Helpers;
using Touride.Framework.Auth;
using Touride.Framework.Caching.Common.CacheManagement;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Repository;
using Touride.Framework.Data.TransactionManagement;
using Touride.Framework.Localization;

namespace Touride.Framework.Api.Extensions
{
    public static class ApiConfigureContainerExtensions
    {
        public static void ConfigureContainer(this ContainerBuilder builder, ApiOptions options)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            //builder.RegisterType<ClientInfoProvider>().As<IClientInfoProvider>().InstancePerLifetimeScope();

            builder.RegisterType<TransactionalInterceptor>().AsSelf();
            builder.RegisterType<CacheInterceptor>().AsSelf();
            builder.RegisterType<ClearCacheInterceptor>().AsSelf();
            builder.RegisterType<HasAuthorityInterceptor>().AsSelf();

            builder.RegisterAssemblyTypes(options.RegistrationAssemblies.ToArray()).
                Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(TransactionalInterceptor))
                .InterceptedBy(typeof(HasAuthorityInterceptor))
                .InterceptedBy(typeof(CacheInterceptor))
                .InterceptedBy(typeof(ClearCacheInterceptor));

            builder.RegisterGeneric(typeof(GenericRepository<,>)).As(typeof(IGenericRepository<,>)).InstancePerDependency();

            builder.RegisterModule<LocalizationModule>();

            builder.RegisterGeneric(typeof(DisconnectedRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();

            builder.RegisterAssemblyTypes(options.RegistrationAssemblies.ToArray()).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();

            if (options.ApiClientList != null)
                foreach (var apiClient in options.ApiClientList)
                {
                    var implementedInterfaces = apiClient.GetInterfaces();
                    if (!implementedInterfaces.Any())
                        builder.Register(scope =>
                        {
                            return GetClientInstance(scope, apiClient);
                        }).As(apiClient);
                    else
                        builder.Register(scope =>
                        {
                            return GetClientInstance(scope, apiClient);
                        }).As(implementedInterfaces.FirstOrDefault());
                }
        }

        private static object GetClientInstance(IComponentContext scope, Type apiClient)
        {
            //Explicitly ensuring the ctor function above is called, and also showcasing why this is an anti-pattern.
            var httpClientFactory = scope.Resolve<IHttpClientFactory>();
            var accessToken = scope.Resolve<Microsoft.AspNetCore.Http.IHttpContextAccessor>().HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var client = httpClientFactory.CreateClient(Regex.Replace(apiClient.Name, "Client$", ""));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //TODO: Clean up both the IServiceINeedToUse and IOtherService configuration here, then somehow rebuild the service tree.
            //Wow!
            if (apiClient.GetConstructors().All(c => c.GetParameters().Length == 2))
                return Activator.CreateInstance(apiClient, new object[] { null, client });
            else
                return Activator.CreateInstance(apiClient, new object[] { client });
        }
    }
}
