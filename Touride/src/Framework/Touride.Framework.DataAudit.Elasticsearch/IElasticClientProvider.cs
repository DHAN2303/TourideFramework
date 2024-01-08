using Nest;

namespace Touride.Framework.DataAudit.Elasticsearch
{
    public interface IElasticClientProvider
    {
        ElasticClient Client { get; }
    }
}
