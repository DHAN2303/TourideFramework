using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Touride.Framework.Auth.OpenApi;

namespace Touride.Framework.Api.OpenApi
{
    /// <summary>
    /// Swagger dokümanları için authorization flow tanımını yapar.
    /// </summary>
    public class ConfigureSwaggerGenerationOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Bağımlılıkları başlatır.
        /// </summary>
        /// <param name="configuration">Konfigürasyondan gelecek identity provider bilgileri için kullanlır</param>
        /// <param name="httpClientFactory">Identity provider keşfi için kullanılır</param>
        public ConfigureSwaggerGenerationOptions(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Swagger içi authorization grant flow konfigürasyonunu yapar
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            //var discoveryDocument = GetDiscoveryDocument();
            string[] ScopesArr;
            options.OperationFilter<AuthorizeOperationFilter>();
            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(x => x.GenericsSupportedId());
            options.SwaggerDoc("v1", CreateOpenApiInfo());

            var identityServerAuthorizationUrl = new Uri($"{_configuration["Touride.Framework:Security:Jwt:Authority"]}/connect/authorize");
            var identityServerTokenUrl = new Uri($"{_configuration["Touride.Framework:Security:Jwt:Authority"]}/connect/token");
            var audience = _configuration["Touride.Framework:Security:Jwt:Audience"];
            var apiName = _configuration["Touride.Framework:Security:Jwt:ApiName"];
            ScopesArr = _configuration.GetSection("Touride.Framework:Security:Jwt:Scopes").Get<string[]>();

            Dictionary<string, string> scopes = new Dictionary<string, string>();

            foreach (var item in ScopesArr)
            {
                scopes.Add(item, item);
            }

            //options.rRootUrl(req => GetRootUrlFromAppConfig());
            options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {

                    Implicit = new OpenApiOAuthFlow()
                    {

                        AuthorizationUrl = identityServerAuthorizationUrl,
                        //TokenUrl = identityServerTokenUrl,
                        Scopes = scopes
                    }
                },
                Description = "Balea Server OpenId Security Scheme"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new[] { "openid", "profile", "email" }
                    }
                });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
        }

        /// <summary>
        /// Identity provider keşfini yapar. JWKS adresi vs.
        /// </summary>
        /// <returns></returns>
        public DiscoveryDocumentResponse GetDiscoveryDocument()
        {
            return _httpClientFactory
                .CreateClient()
                .GetDiscoveryDocumentAsync(_configuration["Touride.Framework:Security:Jwt:Authority"])
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Swagger dokümantasyonu versiyon bilgisi gibi tanımları getirir
        /// </summary>
        /// <returns></returns>
        private OpenApiInfo CreateOpenApiInfo()
        {
            //TODO: Bu kısımlar konfigürasyon dosyasından gelmeli
            return new OpenApiInfo()
            {
                Title = _configuration["Touride.Framework:OpenApi:Name"] ?? "My Awesome API",
                Version = "v1",
                Description = _configuration["Touride.Framework:OpenApi:Name"] ?? "My Awesome API",
                Contact = new OpenApiContact() { Name = "API" },
                License = new OpenApiLicense() { Name = " TG Labs" }
            };
        }
    }
}
