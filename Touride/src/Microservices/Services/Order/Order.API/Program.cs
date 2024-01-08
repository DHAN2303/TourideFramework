using Autofac;
using Autofac.Extensions.DependencyInjection;
using Touride.Framework.Api.Extensions;
using Touride.Framework.Localization;

namespace Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ApiApplicationRunnerExtensions.RunHost((args) => { return CreateHostBuilder(args); }, args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
                    {
                        containerBuilder.RegisterModule<LocalizationModule>();
                    })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}