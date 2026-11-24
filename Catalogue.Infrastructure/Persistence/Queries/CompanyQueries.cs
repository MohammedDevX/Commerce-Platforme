using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Companies.Queries.GetCompanies;
using Catalogue.Application.Companies.Queries.GetCompanyById;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Infrastructure.Persistence.Queries
{
    public class CompanyQueries : ICompanyQueries
    {
        private readonly CatalogueDbContext _context;

        public CompanyQueries(CatalogueDbContext context)
        {
            _context = context;
        }

        public Task<List<CompanyListItemDto>> GetCompaniesAsync(int PageSize, int PageNumber, CancellationToken ct)
        {
            return _context.Companies
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new CompanyListItemDto(c.Id, c.Name))
                .ToListAsync(ct);
        }

        public async Task<CompanyDetailsDto?> GetCompanyByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Companies.Where(c => c.Id == id)
                .Select(c => new CompanyDetailsDto(c.Id, c.Name))
                .SingleOrDefaultAsync(ct);
        }

        public Task<int> GetTotalAsync(CancellationToken ct)
        {
            return _context.Companies.CountAsync(ct);
        }
    }
}
