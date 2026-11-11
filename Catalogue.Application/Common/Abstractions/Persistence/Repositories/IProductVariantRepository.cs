namespace Catalogue.Application.Common.Abstractions.Persistence.Repositories
{
    public interface IProductVariantRepository
    {
        // Get productVariants with Sku 
        public Task<bool> ExistsSkuForProductAsync(string sku, Guid productId, CancellationToken ct);
    }
}
