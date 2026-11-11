using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Products.Queries.GetProductById;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Infrastructure.Persistence.Queries
{
    public class ProductQueries : IProductQueries
    {
        private readonly CatalogueDbContext _context;

        public ProductQueries(CatalogueDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsDto(p.Id, p.CompanyId, p.Name, p.Status))
                .FirstOrDefaultAsync(ct);
        }
    }
}
