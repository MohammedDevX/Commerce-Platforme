using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace Catalogue.Infrastructure.Persistence.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly CatalogueDbContext _context;

        public ProductVariantRepository(CatalogueDbContext context, ICompanyRepository companyRepository)
        {
            _context = context;
        }

        public async Task<bool> ExistsSkuForProductAsync(string sku, Guid productId, CancellationToken ct)
        {
            var product = await _context.Products.Where(c => c.Id == productId).FirstOrDefaultAsync(ct); // ?? Should throw exception here ?

            if (product.ProductVariants.Any(v => v.SKU.SKU == sku))
            {
                return true;
            }

            return false;
        }
    }
}
