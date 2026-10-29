namespace Catalogue.Application.Abstractions.Persistence
{
    public interface IProductVariantRepository
    {
        // Get productVariants with Sku 
        public Task<bool> ExistsSkuForCompanyAsync(string sku, Guid companyId, CancellationToken ct);
    }
}
