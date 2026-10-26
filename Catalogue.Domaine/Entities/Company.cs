namespace Catalogue.Domaine.Entities
{
    public class Company
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        // Empty ctor for EF 
        public Company() {}

        public Company(string companyName)
        {
            Id = Guid.NewGuid();
            Rename(companyName);
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name cannot be empty", nameof(name));

            Name = name.Trim();
        }
    }
}
