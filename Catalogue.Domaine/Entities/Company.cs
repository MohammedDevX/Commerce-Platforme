using Catalogue.Domaine.Enums;

namespace Catalogue.Domaine.Entities
{
    public class Company
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public CompanyStatus Status { get; private set; }
        public IReadOnlyList<Product> Products { get; private set; } = new List<Product>();

        // Empty ctor for EF 
        public Company() { }

        public Company(string companyName)
        {
            Id = Guid.NewGuid();
            Rename(companyName);
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name cannot be empty, null or contain only spaces");

            Name = name.Trim();
        }

        public void Activate()
        {
            if (Status is CompanyStatus.Active)
                return;

            Status = CompanyStatus.Active;
        }

        public void Deactivate()
        {
            if (Status is CompanyStatus.Inactive)
                return;

            Status = CompanyStatus.Inactive;
        }
    }
}
