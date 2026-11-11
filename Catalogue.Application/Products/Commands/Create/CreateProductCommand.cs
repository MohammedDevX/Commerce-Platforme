namespace Catalogue.Application.Products.Commands.Create
{
    public record CreateProductCommand(Guid companyId, string name)
    {}
}
