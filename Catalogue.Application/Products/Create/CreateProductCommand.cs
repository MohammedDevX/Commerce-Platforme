namespace Catalogue.Application.Products.Create
{
    public record CreateProductCommand(Guid companyId, string name)
    {}
}
