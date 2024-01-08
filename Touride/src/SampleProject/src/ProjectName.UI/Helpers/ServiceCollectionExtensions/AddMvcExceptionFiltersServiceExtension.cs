using ProjectName.UI.Helpers.Attributes;

namespace ProjectName.UI.Helpers.ServiceCollectionExtensions
{
    public static class AddMvcExceptionFiltersServiceExtension
    {
        public static void AddMvcExceptionFilters(this IServiceCollection services)
        {
            //Exception handling
            services.AddScoped<ControllerExceptionFilterAttribute>();
        }
    }
}
