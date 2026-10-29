namespace Catalogue.Application.Abstractions.Persistence
{
    public interface IProductVariantRepository
    {
        // Get productVariants with Sku 
        public Task<bool> ExistsSkuForCompanyAsync(string sku, CancellationToken ct);
    }
}
