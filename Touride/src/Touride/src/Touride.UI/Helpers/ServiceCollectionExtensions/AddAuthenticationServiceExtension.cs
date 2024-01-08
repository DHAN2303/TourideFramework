using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Touride.UI.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Touride.UI.Helpers.ServiceCollectionExtensions
{
    public static class AddAuthenticationServiceExtension
    {

        /// <summary>
        /// Register services for authentication, including Identity.
        /// For production mode is used OpenId Connect middleware which is connected to IdentityServer4 instance.
        /// For testing purpose is used cookie middleware with fake login url.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TUserIdentity"></typeparam>
        /// <typeparam name="TUserIdentityRole"></typeparam>
        /// <param name="services"></param>
        /// <param name="adminConfiguration"></param>
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var oidcConfiguration = configuration.GetSection(OidcConfiguration.OidcOptionsSection).Get<OidcConfiguration>();

            IdentityModelEventSource.ShowPII = true;

            services.Configure<OidcConfiguration>(configuration.GetSection(OidcConfiguration.OidcOptionsSection));

            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = AuthenticationConsts.OidcAuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                        options =>
                        {
                            options.Cookie.Name = oidcConfiguration.IdentityAdminCookieName;
                            options.Cookie.SameSite = SameSiteMode.Lax;

                        })
                    .AddOpenIdConnect(AuthenticationConsts.OidcAuthenticationScheme, options =>
                    {
                        options.Authority = oidcConfiguration.IdentityServerBaseUrl;
                        options.RequireHttpsMetadata = oidcConfiguration.RequireHttpsMetadata;
                        options.ClientId = oidcConfiguration.ClientId;
                        options.ClientSecret = oidcConfiguration.ClientSecret;
                        options.ResponseType = oidcConfiguration.OidcResponseType;

                        options.SecurityTokenValidator = new JwtSecurityTokenHandler
                        {
                            InboundClaimTypeMap = new Dictionary<string, string>()
                        };

                        options.Scope.Clear();
                        foreach (var scope in oidcConfiguration.Scopes)
                        {
                            options.Scope.Add(scope);
                        }

                        options.ClaimActions.Add(new JsonKeyClaimAction("role", "role", "role"));
                        //options.ClaimActions.Add(new JsonKeyClaimAction("accounttype", null, "accounttype"));

                        options.SaveTokens = true;

                        options.GetClaimsFromUserInfoEndpoint = true;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = oidcConfiguration.TokenValidationClaimName,
                            RoleClaimType = oidcConfiguration.TokenValidationClaimRole,
                        };

                        options.Events = new OpenIdConnectEvents
                        {
                            OnMessageReceived = context => OnMessageReceived(context, oidcConfiguration),
                            OnRedirectToIdentityProvider = context => OnRedirectToIdentityProvider(context, oidcConfiguration),
                            OnTokenValidated = context => OnTokenValidated(context)
                        };
                    });

            return services;
        }


        private static Task OnRedirectToIdentityProvider(RedirectContext context, OidcConfiguration adminConfiguration)
        {
            if (!string.IsNullOrEmpty(adminConfiguration.IdentityAdminRedirectUri))
            {
                context.ProtocolMessage.RedirectUri = adminConfiguration.IdentityAdminRedirectUri;
            }

            return Task.CompletedTask;
        }
        private static Task OnMessageReceived(MessageReceivedContext context, OidcConfiguration adminConfiguration)
        {
            context.Properties.IsPersistent = true;
            context.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(adminConfiguration.IdentityAdminCookieExpiresUtcHours));

            return Task.CompletedTask;
        }


        private static Task OnTokenValidated(TokenValidatedContext context)
        {
            var token = context.TokenEndpointResponse.AccessToken;

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            var id = context.Principal.Identity as ClaimsIdentity;

            foreach (var claim in jsonToken.Claims)
            {
                var control = id.Claims.FirstOrDefault(x => x.Type == claim.Type && x.Value == x.Value);
                if (control is null)
                    id.AddClaim(claim);
            }

            return Task.CompletedTask;
        }
        public static void UseSecurityHeaders(this IApplicationBuilder app)
        {
            var forwardingOptions = new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.All
            };

            forwardingOptions.KnownNetworks.Clear();
            forwardingOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardingOptions);
        }

    }
}