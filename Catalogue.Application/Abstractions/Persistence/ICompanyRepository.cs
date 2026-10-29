using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Abstractions.Persistence
{
    public interface ICompanyRepository
    {
        public Task<Company> GetCompanyByIdAsync(Guid companyId, CancellationToken ct);
    }
}
