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
            Id = Guid.NewGuid();
            CompanyId = companyId;
            Name = productName;
        }
    }
}
