using Catalogue.Domaine.Entities;
using Docker.DotNet.Models;

namespace Catalogue.Tests.Common.Builders
{
    public class CompanyBuilder
    {
        private string _Name = "Razor";

        public CompanyBuilder WithName(string name)
        {
            _Name = name;
            return this;
        }

        public Company Build()
        {
            return new Company(_Name);
        }
    }
}
