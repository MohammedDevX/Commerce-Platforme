using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Common.Abstractions.Persistence
{
    public interface ICompanyRepository
    {
        public Task<Company?> GetCompanyByIdAsync(Guid companyId, CancellationToken ct);
        public Task<bool> IsExists(Guid companyId, CancellationToken ct);
    }
}
