using Catalogue.Domaine.Entities.ValueObjects;
using System.Text.RegularExpressions;

namespace Catalogue.Domaine.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public Company Company { get; set; }
        private readonly List<ProductVariant> _variants = new();
        // This syntax is an explisite {get => return _variants;}
        /* 
         * And .AsReadOnly() wrap the original collection into ReadOnlyCollection so if someone outside of
         * the class try to modify in the collection an InvalidCastException is thrown
         */
        public IReadOnlyCollection<ProductVariant> ProductVariants => _variants.AsReadOnly();

        private Product()
        {
        }

        public Product(Guid companyId, string productName) 
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("CompanyId cannot by empty");

            Rename(productName);

            Id = Guid.NewGuid();
            CompanyId = companyId;
        }

        public void Rename(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product Name cannot be empty, null or contain only spaces");

            Name = productName.Trim();
        }

        public void AddProductVariant(Money price, Sku sku)
        {
            ProductVariant productVariant = new(Id, price, sku);
            _variants.Add(productVariant);
        }
    }
}
