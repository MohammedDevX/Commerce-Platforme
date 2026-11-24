namespace Catalogue.Application.Companies.Queries.GetCompanies
{
    public sealed record GetCompaniesQuery(int pageSize = 20, int pageNumber = 1)
    {
    }
}
