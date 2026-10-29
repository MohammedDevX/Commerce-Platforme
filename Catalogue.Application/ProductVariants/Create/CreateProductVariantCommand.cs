using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Application.ProductVariants.Create
{
    public record CreateProductVariantCommand
    {
        public Guid ProductId { get; init; }
        public decimal Price { get; init; }
        public required string Sku { get; init; }
    }
}
