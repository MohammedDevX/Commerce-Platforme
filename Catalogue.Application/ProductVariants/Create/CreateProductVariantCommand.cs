using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Application.ProductVariants.Create
{
    public record CreateProductVariantCommand
    {
        public Guid ProductId { get; set; }
        public Money Price { get; set; }
        public string Sku { get; set; }
    }
}
