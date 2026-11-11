using Catalogue.Application.Products.Queries.GetProductById;

namespace Catalogue.Application.Common.Abstractions.Persistence.Queries
{
    public interface IProductQueries
    {
        public Task<ProductDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
