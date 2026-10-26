namespace Catalogue.Domaine.Entities
{
    public class Company
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        public Company(string CompanyName)
        {
            Id = Guid.NewGuid();
            Rename(CompanyName);
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name cannot be emoty", nameof(name));

            Name = name.Trim();
        }
    }
}
