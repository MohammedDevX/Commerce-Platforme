using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Catalogue.Domaine.Entities.ValueObjects
{
    public record Sku
    {
        public string SkuChaine { get; set; }

        public Sku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be null or empty or contains only spaces", nameof(sku));

            if((bool isValid, string normalizerSku)IsFormatValide(sku)) {

            }

        }

        private (bool, string) IsFormatValide(string sku)
        {
            sku.ToUpper().Normalize().Trim();
            return (Regex.IsMatch(sku, @"^[a-zA-Z0-9_-]+$"), sku);
        }
    }
}
