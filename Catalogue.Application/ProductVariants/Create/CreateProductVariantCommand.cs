using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Application.ProductVariants.Create
{
    public record CreateProductVariantCommand(Guid ProductId, decimal Price, string Sku)
    {
    }
}
