using Autofac;
using Microsoft.Extensions.Localization;
using Touride.Framework.Localization.Dictionary;
using Touride.Framework.Localization.Resources.Validation;

namespace Touride.Framework.Localization
{
    public class LocalizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DictionaryLocalizerFactory>().As<IStringLocalizerFactory>().SingleInstance();
            builder.RegisterType<ValidationResource>().As<IStringLocalizer<ValidationResource>>().SingleInstance();
        }
    }
}