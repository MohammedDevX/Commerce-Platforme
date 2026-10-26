using System.Text.RegularExpressions;


namespace Catalogue.Domaine.Entities.ValueObjects
{
    public record Sku
    {
        public string SkuChaine { get; set; }

        public Sku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be null or empty or contains only spaces", nameof(sku));

            var skuFormatValidation = IsFormatValide(sku);
            if (skuFormatValidation.isValid) {
                sku = skuFormatValidation.normalizerSku;
            }

        }

        private (bool isValid, string normalizerSku) IsFormatValide(string sku)
        {
            sku.ToUpper().Normalize().Trim();
            return (Regex.IsMatch(sku, @"^[a-zA-Z0-9_-]+$"), sku);
        }
    }
}
