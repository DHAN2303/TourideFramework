using Nest;

namespace Touride.Framework.Elasticsearch.Services
{
    public interface IElasticSearchService<T> where T : class
    {
        IndexResponse Create(T entity);
        T Get(string id);
        UpdateResponse<T> Update(string id, T entity);
        DeleteResponse Delete(string id);
        IEnumerable<T> Search(string queryText);
    }
}
