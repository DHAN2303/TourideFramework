namespace Product.Abstraction.Dtos
{
    public class CatalogItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string PictureFileName { get; set; }

        public Guid CatalogTypeId { get; set; }

        public CatalogTypeDto CatalogType { get; set; } = null!;

        public Guid CatalogBrandId { get; set; }

        public CatalogBrandDto CatalogBrand { get; set; } = null!;

        public int AvailableStock { get; set; }
    }
}
