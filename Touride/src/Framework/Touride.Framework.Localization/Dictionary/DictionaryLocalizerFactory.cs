using Microsoft.Extensions.Localization;
using System;

namespace Touride.Framework.Localization.Dictionary
{
    public class DictionaryLocalizerFactory : IStringLocalizerFactory
    {
        public IServiceProvider ServiceProvider;

        public DictionaryLocalizerFactory(IServiceProvider provider)
        {
            ServiceProvider = provider;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var registeredLocalizer = typeof(IStringLocalizer<>).MakeGenericType(resourceSource);

            var registeredServices = ServiceProvider.GetService(registeredLocalizer);

            if (registeredServices != null)
                return registeredServices as IStringLocalizer;

            return Activator.CreateInstance(resourceSource) as IStringLocalizer;
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return null;
        }
    }
}