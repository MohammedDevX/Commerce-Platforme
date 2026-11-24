using Catalogue.Application.Companies.Queries.GetCompanies;
using Catalogue.Application.Companies.Queries.GetCompanyById;

namespace Catalogue.Application.Common.Abstractions.Persistence.Queries
{
    public interface ICompanyQueries
    {
        public Task<CompanyDetailsDto?> GetCompanyByIdAsync(Guid id, CancellationToken ct);
        public Task<List<CompanyListItemDto>> GetCompaniesAsync(int PageSize, int PageNumber, CancellationToken ct);
        public Task<int> GetTotalAsync(CancellationToken ct);
    }
}