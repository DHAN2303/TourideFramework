using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Abstractions.Data.Entities;

namespace Product.Domain.AggregatesModel.CatalogTypeAggregate
{
    public class CatalogType : IHasId, IHasFullAudit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}