using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Catalogue.Application.Companies.Queries.GetCompanies
{
    public class GetCompaniesHandler
    {
        private readonly ICompanyQueries _companyQueries;

        public GetCompaniesHandler(ICompanyQueries companyQueries)
        {
            _companyQueries = companyQueries;
        }

        public async Task<PageResult<CompanyListItemDto>> Handle(GetCompaniesQuery query, CancellationToken ct)
        {
            if (query.pageNumber <= 0)
                throw new ValidationException($"Invalid argument {nameof(query.pageNumber)}");

            if (query.pageSize <= 0 || query.pageSize > 100)
                throw new ValidationException($"Invalid argument {nameof(query.pageSize)}");

            var result = await _companyQueries.GetCompaniesAsync(query.pageSize, query.pageNumber, ct);
            var totalCount = await _companyQueries.GetTotalAsync(ct);

            return new PageResult<CompanyListItemDto>(result, query.pageNumber, totalCount, query.pageSize);
        }
    }
}