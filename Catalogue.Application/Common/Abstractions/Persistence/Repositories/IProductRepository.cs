using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Common.Abstractions.Persistence.Repositories
{
    public interface IProductRepository
    {
        public Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken ct);
        public void Add(Product product);
    }
}
