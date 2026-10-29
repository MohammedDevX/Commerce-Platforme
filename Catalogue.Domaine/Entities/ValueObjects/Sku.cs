using System.Text.RegularExpressions;

namespace Catalogue.Domaine.Entities.ValueObjects
{
    public record Sku
    {
        public string SKU { get; private init; }

        public Sku(string sku)
        {
            var skuFormatValidation = IsFormatValide(sku);
            if (!skuFormatValidation.isValid)
                throw new ArgumentException("Sku format is invalide");

            SKU = skuFormatValidation.normalizedSku!;
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
