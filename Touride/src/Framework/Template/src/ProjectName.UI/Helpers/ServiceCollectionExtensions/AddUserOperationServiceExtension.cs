using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ProjectName.UI.Helpers.ServiceCollectionExtensions
{
    public static class AddUserOperationServiceExtensions
    {
        public static IServiceCollection AddUserOperationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(x =>
            {
                var baseUrl = configuration.GetValue<string>("Apis:TestProject");
                var context = x.GetService<IHttpContextAccessor>();

                var accessToken = context.HttpContext?.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;

                var httpClient = new HttpClient();
                httpClient.SetBearerToken(accessToken);

                return new ProjectName.Api.ProjectNameApi(baseUrl, httpClient);
            });

            return services;
        }
    }
}
