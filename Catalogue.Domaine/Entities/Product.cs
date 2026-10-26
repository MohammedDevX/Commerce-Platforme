using System.Globalization;

namespace Catalogue.Domaine.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public string Name { get; private set; } = string.Empty;

        public Product(Guid CompanyId, string ProductName) 
        {
            Id = Guid.NewGuid();
            this.CompanyId = CompanyId;
            Name = ProductName;
        }
    }
}
