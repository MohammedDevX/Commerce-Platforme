using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Tests.Common.Builders
{
    public class ProductBuilder
    {
        private Guid _CompanyId = Guid.NewGuid();
        private string _Name = "Razor Mouse";
        private bool _InActive;
        // Tupled List
        private List<(string sku, decimal price)> _Variants = new();
        
        public ProductBuilder WithName(string name)
        {
            _Name = name;
            return this;
        }

        public ProductBuilder WithCompanyId(Guid companyId)
        {
            _CompanyId = companyId;
            return this;
        }

        public ProductBuilder WithInactive(Guid companyId)
        {
            _InActive = true;
            return this;
        }

        public ProductBuilder WithVariant(string sku , decimal price)
        {
            _Variants.Add((sku, price));
            return this;
        }

        public Product Build()
        {
            var product = new Product(_CompanyId, _Name);

            if (_Variants.Count > 0)
            {
                foreach (var item in _Variants)
                {
                    product.AddProductVariant(new Money(item.price), new Sku(item.sku));
                }
            }

            if (_InActive)
                product.DesactivateProduct();

            return product;
        }
    }
}
