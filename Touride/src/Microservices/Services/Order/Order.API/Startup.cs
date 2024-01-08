using Autofac;
using Order.API.Helpers;
using Order.Application.Actors;
using System.Reflection;
using Touride.Framework.Api.Extensions;
using Touride.Framework.Api.Helpers;
using Touride.Framework.DataAudit.Common;

namespace Order.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
            Assemblies = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly)
               .Where(filePath => Path.GetFileName(filePath).StartsWith("Order"))
               .Select(Assembly.LoadFrom);
        }
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        private IEnumerable<Assembly> Assemblies { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection builder)
        {
            builder.AddDefactoApi(Configuration, new ApiOptions
            {
                RegistrationAssemblies = Assemblies,
                AuditLogStoreType = AuditLogStoreType.PostgreSql,
                ConnectionString = Configuration.GetConnectionString("SampleConnectionstring"),
                MigrationsAssemblyName = "Order.Infrastructure",
            });

            builder.AddActors(options =>
            {
                options.Actors.RegisterActor<OrderingProcessActor>();
            });

        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureContainer(new ApiOptions() { RegistrationAssemblies = Assemblies, ApiClientList = null });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation($"Using Environment: {env.EnvironmentName}");

            app.UseDefactoApi(
                        new ApiOptions()
                        {
                            HubList = new List<Type>(),
                            ApiName = Configuration["Touride.Framework:OpenApi:Name"],
                            RegistrationAssemblies = Assemblies,
                        },
                        Configuration,
                        WebHostEnvironment);

        }
    }
}