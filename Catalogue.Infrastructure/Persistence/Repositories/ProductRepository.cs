using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Domaine.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogueDbContext _context;
        public ProductRepository(CatalogueDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken ct)
        {
            return await _context.Products.Where(p => p.Id == productId).FirstOrDefaultAsync(ct);
        }
    }
}
