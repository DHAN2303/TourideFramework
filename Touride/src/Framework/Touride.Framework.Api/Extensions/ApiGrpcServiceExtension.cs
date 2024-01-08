using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Reflection;
using Touride.Framework.Api.Helpers;

namespace Touride.Framework.Api.Extensions
{
    public static class ApiGrpcServiceExtension
    {
        public static void AddGrpcServices(IEndpointRouteBuilder builder, IEnumerable<Assembly> assembly)
        {

            var assemblyName = GetAssembly(assembly, "Api").GetName().Name;

            foreach (var item in GrpcServicesHelper.GetGrpcServices(assemblyName))
            {
                Type mytype = GetAssembly(assembly, "Api")
                        .GetType(item.Value + "." + item.Key);

                var method = typeof(GrpcEndpointRouteBuilderExtensions).GetMethod("MapGrpcService").MakeGenericMethod(mytype);

                method.Invoke(null, new[] { builder });
            };
        }
        public static void useGrpcServices(this IApplicationBuilder app, IEnumerable<Assembly> assembly)
        {
            app.UseEndpoints(endpoints =>
            {
                AddGrpcServices(endpoints, assembly);
            });
        }

        private static Assembly GetAssembly(IEnumerable<Assembly> assembly, string apiName)
        {
            return assembly.FirstOrDefault(f => f.GetName().Name.EndsWith(apiName));
        }

    }
}
