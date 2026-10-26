using Catalogue.Domaine.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domaine.Entities
{
    public class ProductVariant
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Money? Price { get; private set; }
        public string Sku { get; private set; } = string.Empty;
        public Product Product { get; set; }

        public ProductVariant()
        {}

        public ProductVariant(Guid productId, Money? price)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("ProductId cannot by empty", nameof(productId));

            Id = Guid.NewGuid();
            ProductId = productId;
            Price = price;
        }
    }
}
