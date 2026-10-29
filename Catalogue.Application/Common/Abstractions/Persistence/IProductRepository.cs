using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Common.Abstractions.Persistence
{
    public interface IProductRepository
    {
        public Task<Product> GetProductByIdAsync(Guid productId, CancellationToken ct);
        // The aggregat root is responsible to add ProductVariant
        //public Task AddProductVariantAsync(ProductVariant productVariant, CancellationToken ct);
    }
}
