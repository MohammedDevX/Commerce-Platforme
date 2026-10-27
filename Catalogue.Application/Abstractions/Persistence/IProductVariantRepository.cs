namespace Catalogue.Application.Abstractions.Persistence
{
    public interface IProductVariantRepository
    {
        // Get productVariants with Sku 
        public Task<bool> GetProductVariantsBySku(string sku);
    }
}
