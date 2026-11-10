using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Domaine.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CatalogueDbContext _context;
        public CompanyRepository(CatalogueDbContext context)
        {
            _context = context;
        }

        public async Task<Company?> GetCompanyByIdAsync(Guid companyId, CancellationToken ct)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId, ct);
        }

        public async Task<bool> ExistsAsync(Guid companyId, CancellationToken ct)
        {
            return await _context.Companies.AsNoTracking().AnyAsync(c => c.Id == companyId, ct);
        }
    }
}
