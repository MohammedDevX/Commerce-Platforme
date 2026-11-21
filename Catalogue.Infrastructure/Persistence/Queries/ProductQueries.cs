using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Products.Queries.GetProductById;
using Catalogue.Application.Products.Queries.GetProducts;
using Catalogue.Infrastructure.Data;
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

        public async Task<List<ProductListItemDto>> GetAllAsync(int pageSize, int pageNumber, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductListItemDto(p.Id, p.Name, p.Status.ToString()))
                .ToListAsync(ct);
        }

        public async Task<ProductDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsDto(p.Id, p.CompanyId, p.Name, p.Status.ToString()))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<int> GetProductsCountAsync(CancellationToken ct)
        {
            return await _context.Products.CountAsync(ct);
        }
    }
}
