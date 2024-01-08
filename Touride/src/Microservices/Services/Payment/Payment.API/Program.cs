using Autofac.Extensions.DependencyInjection;
using Touride.Framework.Api.Extensions;

namespace Payment.API
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
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}