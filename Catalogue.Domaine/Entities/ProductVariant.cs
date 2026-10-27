using Catalogue.Domaine.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Catalogue.Domaine.Entities
{
    public class ProductVariant
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Money Price { get; private set; }
        public string Sku { get; private set; } = string.Empty;
        public Product Product { get; set; }

        public ProductVariant()
        {}

        public ProductVariant(Guid productId, Money price, string sku)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("ProductId cannot by empty", nameof(productId));

            Id = Guid.NewGuid();
            ProductId = productId;
            Price = price;

            var skuFormatValidation = IsFormatValide(sku);
            if (skuFormatValidation.isValid == false)
                throw new ArgumentException("Sku format is invalide", nameof(sku));

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
