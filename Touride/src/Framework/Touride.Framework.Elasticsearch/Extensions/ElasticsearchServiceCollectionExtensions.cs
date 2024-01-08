using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Touride.Framework.Elasticsearch.Services;

namespace Touride.Framework.Elasticsearch.Extensions
{
    public static class ElasticsearchServiceCollectionExtensions
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new Uri(configuration["Touride.Framework:ElasticSearch:Url"]));
            var client = new ElasticClient(settings);

            services.AddSingleton(client);

            services.AddSingleton(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));

            return services;
        }
    }
}
