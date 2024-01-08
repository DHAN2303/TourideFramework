using Product.Domain.AggregatesModel.CatalogBrandAggregate;
using Product.Domain.AggregatesModel.CatalogTypeAggregate;
using Touride.Framework.Abstractions.Data.AuditProperties;
using Touride.Framework.Abstractions.Data.Entities;

namespace Product.Domain.AggregatesModel.CatalogItemAggregate
{
    public class CatalogItem : IHasId, IHasFullAudit
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string PictureFileName { get; set; }

        public Guid CatalogTypeId { get; set; }

        public CatalogType CatalogType { get; set; } = null!;

        public Guid CatalogBrandId { get; set; }

        public CatalogBrand CatalogBrand { get; set; } = null!;

        public int AvailableStock { get; set; }


    }
}