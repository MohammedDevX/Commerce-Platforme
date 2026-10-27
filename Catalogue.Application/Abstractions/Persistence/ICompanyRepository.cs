namespace Catalogue.Application.Abstractions.Persistence
{
    public interface IProductVariantRepository
    {
        // Should be in IProductRepository ?
        public Task<bool> GetProductById(string productId);
        // Get productVariants with Sku 
        public Task<bool> GetProductVariantBySku(string sku);
    }
}
