using Catalogue.Domaine.Entities.ValueObjects;
using System.Text.RegularExpressions;

namespace Catalogue.Domaine.Entities
{
    public class ProductVariant
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Money Price { get; private set; }
        public Sku SKU { get; private set; }
        public Product Product { get; set; }

        private ProductVariant()
        {}

        public ProductVariant(Guid productId, Money price, Sku sku)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("ProductId cannot by empty");

            SKU = sku;

            if (price is null)
                throw new ArgumentException("Price cannot be null");

            Id = Guid.NewGuid();
            ProductId = productId;
            Price = price;
        }
    }
}
