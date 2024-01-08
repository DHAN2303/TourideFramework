using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Touride.Framework.Abstractions.Secrets;

namespace Touride.Framework.Secret.Providers
{
    public class AzureKeyVaultProvider : IVaultProvider
    {
        private readonly IConfiguration _configuration;
        public AzureKeyVaultProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetValue(string mySecret)
        {

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                    {
                        Delay= TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = RetryMode.Exponential
                     }
            };

            string userAssignedClientId = _configuration["Touride.Framework:AzureKeyVault:ClientId"];

            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = userAssignedClientId });


            var client = new SecretClient(new Uri(_configuration["Touride.Framework:AzureKeyVault:Url"]), credential, options);

            KeyVaultSecret secret = client.GetSecret(mySecret);

            string secretValue = secret.Value;

            return secretValue;

        }
    }
}
