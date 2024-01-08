using Nest;

namespace Touride.Framework.Elasticsearch.Services
{
    public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
    {
        private readonly IElasticClient _client;
        private readonly string _indexName;

        public ElasticSearchService(IElasticClient client)
        {
            _client = client;
            _indexName = typeof(T).Name.ToLower(); // Her model için ayrı bir index ismi kullanılmıştır.
        }

        // Create Operation
        public IndexResponse Create(T entity)
        {
            return _client.Index(entity, idx => idx.Index(_indexName));
        }

        // Read Operation
        public T Get(string id)
        {
            var response = _client.Get<T>(id, g => g.Index(_indexName));
            return response?.Source;
        }

        // Update Operation
        public UpdateResponse<T> Update(string id, T entity)
        {
            return _client.Update<T>(id, u => u
                .Index(_indexName)
                .Doc(entity)
            );
        }

        // Delete Operation
        public DeleteResponse Delete(string id)
        {
            return _client.Delete<T>(id, d => d.Index(_indexName));
        }

        // Search Operation
        public IEnumerable<T> Search(string queryText)
        {
            var response = _client.Search<T>(s => s
                .Index(_indexName)
                .Query(q => q
                    .Match(m => m
                        .Field("_all")
                        .Query(queryText)
                    )
                )
            );

            return response.Documents;
        }
    }
}
