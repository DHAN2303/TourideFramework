using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Touride.Api.Helpers;
using Touride.Framework.Api.Extensions;
using Touride.Framework.Localization;

namespace Touride.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ApiApplicationRunnerExtensions.RunHost((args) => { return CreateHostBuilder(args); }, args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configBuilder) =>
            {
                if (hostingContext.HostingEnvironment.IsProduction()) configBuilder.AddCustomConfiguration();
            })
            .ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterModule<LocalizationModule>();
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}