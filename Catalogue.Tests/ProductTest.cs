using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;
using System.Linq;

namespace Catalogue.Tests
{
    public class ProductTest
    {
        [Fact]
        public void Constructor_WithValidData_InitializesProduct()
        {
            Product product = new(Guid.NewGuid(), "Razor mouse");
            Assert.Equal("Razor mouse", product.Name);
        }

        [Fact]
        public void Should_PoroductName_Be_Normalized_Test()
        {
            string productName = " Razor Mouse ";
            Assert.Equal("Razor Mouse", new Product(Guid.NewGuid(), productName).Name);
        }

        [Fact]
        public void Should_Throw_Exception_If_CompanyId_Is_Null_Test()
        {
            Assert.Throws<ArgumentException>(() => new Product(Guid.Empty, "Razor Mouse"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void Should_Poroduct_Name_Dont_Be_Null_Empty_Or_Contain_Only_Spaces_Test(string productName)
        {
            Assert.Throws<ArgumentException>(() => new Product(Guid.NewGuid(), productName));
        }

        [Fact]
        public void Should_Product_Add_Variant_Test()
        {
            Product product = new(Guid.NewGuid(), "Razor Mouse");
            product.AddProductVariant(new Money(140m), "RAZOR-101");
            Assert.Single(product.ProductVariants);
            Assert.True(product.ProductVariants?.Any(p => p.Sku == "RAZOR-101"));
        }
    }
}
