using Catalogue.Domaine.Entities.ValueObjects;
using System.Text.RegularExpressions;

namespace Catalogue.Domaine.Entities
{
    public class ProductVariant
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Money Price { get; private set; }
        public string Sku { get; private set; } = string.Empty;
        public Product Product { get; set; }

        private ProductVariant()
        {}

        public ProductVariant(Guid productId, Money price, string sku)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("ProductId cannot by empty");

            var skuFormatValidation = IsFormatValide(sku);
            if (skuFormatValidation.isValid == false)
                throw new ArgumentException("Sku format is invalide");

            if (price is null)
                throw new ArgumentException("Price cannot be null");

            Id = Guid.NewGuid();
            ProductId = productId;
            Price = price;
            Sku = skuFormatValidation.normalizedSku!;
        }

        private (bool isValid, string? normalizedSku) IsFormatValide(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                return (false, null);

            sku = sku.Trim().ToUpperInvariant().Normalize();
            return (Regex.IsMatch(sku, @"^[a-zA-Z0-9_-]+$"), sku);
        }
    }
}
