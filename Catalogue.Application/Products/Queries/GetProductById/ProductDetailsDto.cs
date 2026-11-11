using Catalogue.Domaine.Enums;

namespace Catalogue.Application.Products.Queries.GetProductById
{
    public sealed record ProductDetailsDto(Guid Id, Guid CompanyId, string Name, ProductStatus ProductStatus)
    {}
}
