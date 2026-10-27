using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Abstractions.Persistence
{
    public interface IProductRepository
    {
        public Task<Product> GetProductById(Guid productId);
        // The aggregat root is responsible to add ProductVariant
        public Task AddProductVariant(ProductVariant productVariant);
    }
}
