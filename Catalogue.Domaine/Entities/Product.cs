using System.Text.RegularExpressions;

namespace Catalogue.Domaine.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public Company Company { get; set; }
        public IReadOnlyList<ProductVariant> ProductVariants { get; private set; } = new List<ProductVariant>();

        public Product()
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
    }
}
