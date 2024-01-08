using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Abstractions.Secrets;
using Touride.Framework.Secret.Providers;

namespace Touride.Framework.Secret.Extensions
{
    public static class AzureKeyVaultServiceCollectionExtensions
    {
        /// <summary>
        /// Keyvault konfigurasyonunun kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static void ConfigureAzureKeyVault(this IServiceCollection services)
        {
            services.AddSingleton<IVaultProvider, AzureKeyVaultProvider>();
        }

        public static IVaultProvider GetVaultProvider(this IServiceCollection services)
        {
            using ServiceProvider serviceProvider = services.BuildServiceProvider(validateScopes: true);

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var secretService = scope.ServiceProvider.GetRequiredService<IVaultProvider>();

                return secretService;
            }
        }
    }
}
