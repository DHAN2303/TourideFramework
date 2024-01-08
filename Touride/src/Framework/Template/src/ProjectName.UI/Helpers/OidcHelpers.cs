using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using ProjectName.UI.Configurations;

namespace ProjectName.UI.Helpers
{
    public class OidcHelpers
    {
        private static Task OnMessageReceived(MessageReceivedContext context, OidcConfiguration adminConfiguration)
        {
            context.Properties.IsPersistent = true;
            context.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(adminConfiguration.IdentityAdminCookieExpiresUtcHours));

            return Task.CompletedTask;
        }

        private static Task OnRedirectToIdentityProvider(RedirectContext context, OidcConfiguration adminConfiguration)
        {
            if (!string.IsNullOrEmpty(adminConfiguration.IdentityAdminRedirectUri))
            {
                context.ProtocolMessage.RedirectUri = adminConfiguration.IdentityAdminRedirectUri;
            }

            return Task.CompletedTask;
        }
    }
}
