using System.Globalization;

namespace Catalogue.Domaine.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public string Name { get; private set; } = string.Empty;

        public Product()
        {}

        public Product(Guid companyId, string productName) 
        {
            Id = Guid.NewGuid();
            CompanyId = companyId;
            Name = productName;
        }
    }
}
