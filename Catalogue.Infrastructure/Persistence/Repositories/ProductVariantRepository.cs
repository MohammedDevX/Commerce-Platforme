using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Domaine.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Infrastructure.Persistence.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly CatalogueDbContext _context;

        public ProductVariantRepository(CatalogueDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsSkuForProductAsync(string sku, Guid productId, CancellationToken ct)
        {
            return await _context.ProductVariants.AsNoTracking().
                Where(v => v.ProductId == productId && v.SKU == new Sku(sku)).AnyAsync(ct);
        }
    }
}
