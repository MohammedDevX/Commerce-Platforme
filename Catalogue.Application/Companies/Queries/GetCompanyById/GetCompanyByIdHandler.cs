using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdHandler
    {
        private readonly ICompanyQueries _companyQueries;

        public GetCompanyByIdHandler(ICompanyQueries companyQueries)
        {
            _companyQueries = companyQueries;
        }

        public async Task<CompanyDetailsDto> Handle(GetCompanyByIdQuery command, CancellationToken ct)
        {
            return await _companyQueries.GetCompanyByIdAsync(command.Id, ct) 
                ?? throw new NotFoundException(nameof(Company), command.Id);
        }
    }
}
