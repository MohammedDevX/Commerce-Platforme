using Catalogue.Application.Products.Queries.GetProductById;
using Catalogue.Application.Products.Queries.GetProducts;

namespace Catalogue.Application.Common.Abstractions.Persistence.Queries
{
    public interface IProductQueries
    {
        public Task<ProductDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct);
        public Task<List<ProductListItemDto>> GetAllAsync(int pageSize, int pageNumber, CancellationToken ct);
        public Task<int> GetProductsCountAsync(CancellationToken ct);
    }
}
