using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Application.ProductVariants.Commands.Create
{
    public record CreateProductVariantCommand(Guid ProductId, decimal Price, string Sku)
    {
    }
}
