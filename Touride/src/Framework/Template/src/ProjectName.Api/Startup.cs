using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectName.Api.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Touride.Framework.Api.Extensions;
using Touride.Framework.Api.Helpers;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Repository;
using Touride.Framework.DataAudit.Common;
using Touride.Framework.Statics.Services;

namespace ProjectName.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
            Assemblies = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly)
               .Where(filePath => Path.GetFileName(filePath).StartsWith("ProjectName"))
               .Select(Assembly.LoadFrom);
        }
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        private IEnumerable<Assembly> Assemblies { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDefactoApi(Configuration, new ApiOptions
            {
                RegistrationAssemblies = Assemblies,
                AuditLogStoreType = AuditLogStoreType.Elastic,
                ConnectionString = Configuration.GetConnectionString("SampleConnectionstring"),
                MigrationsAssemblyName = "ProjectName.Infrastructure",
            });



            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureContainer(new ApiOptions() { RegistrationAssemblies = Assemblies, ApiClientList = null });
            builder.RegisterGeneric(typeof(StaticService<>)).As(typeof(IStaticService<>)).InstancePerDependency();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation($"Using Environment: {env.EnvironmentName}");
            logger.LogInformation($"URL : {Configuration.GetValue<string>("Touride.Framework:OpenApi:Name")}");
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