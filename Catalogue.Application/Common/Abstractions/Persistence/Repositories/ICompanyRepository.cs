using Catalogue.Domaine.Entities;
using System.Runtime.InteropServices;

namespace Catalogue.Application.Common.Abstractions.Persistence.Repositories
{
    public interface ICompanyRepository
    {
        public Task<Company?> GetCompanyByIdAsync(Guid companyId, CancellationToken ct);
        public Task<bool> ExistsAsync(Guid companyId, CancellationToken ct);
        public void Add(Company company, CancellationToken ct);
        public void Delete(Company company);
    }
}
