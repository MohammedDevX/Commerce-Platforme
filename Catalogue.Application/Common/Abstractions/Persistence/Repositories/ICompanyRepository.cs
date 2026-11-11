using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Common.Abstractions.Persistence.Repositories
{
    public interface ICompanyRepository
    {
        public Task<Company?> GetCompanyByIdAsync(Guid companyId, CancellationToken ct);
        public Task<bool> ExistsAsync(Guid companyId, CancellationToken ct);
    }
}
