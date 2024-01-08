using Microsoft.Extensions.DependencyInjection;
using Touride.Framework.Statics.Services;

namespace Touride.Framework.Statics.Exceptions
{
    public static class StaticServiceCollectionExtensions
    {
        /// <summary>
        /// konfigurasyon kaydı için kullanılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static void AddStatics<T>(this IServiceCollection services)
        {
            services.AddScoped<IStaticService<T>, StaticService<T>>();
        }

    }
}
