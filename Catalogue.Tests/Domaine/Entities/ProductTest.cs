using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;
using Catalogue.Domaine.Enums;
using System.Linq;

namespace Catalogue.Tests.Domaine.Entities
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
            Sku sku = new("RAZOR-101");
            product.AddProductVariant(new Money(140m), sku);
            Assert.Single(product.ProductVariants);
            Assert.True(product.ProductVariants?.Any(p => p.SKU.SKU == "RAZOR-101"));
        }

        [Fact]
        public void Activate_ProductSatatus_Test()
        {
            Product product = new(Guid.NewGuid(), "Razor Mouse");
            product.DesactivateProduct();
            product.ActivateProduct();

            Assert.Equal(ProductStatus.Activate, product.Status);
        }

        [Fact]
        public void Desactivate_ProductSatatus_Test()
        {
            Product product = new(Guid.NewGuid(), "Razor Mouse");
            product.DesactivateProduct();

            Assert.Equal(ProductStatus.Desactivate, product.Status);
        }

        [Fact]
        public void Should_Throw_Exception_If_Product_IsActivate_And_Call_ActivateProduct()
        {
            Product product = new(Guid.NewGuid(), "Razor Mouse");
            Assert.Throws<InvalidOperationException>(() => product.ActivateProduct());
        }

        [Fact]
        public void Should_Throw_Exception_If_Product_IsDesactivate_And_Call_DesactivateProduct()
        {
            Product product = new(Guid.NewGuid(), "Razor Mouse");
            product.DesactivateProduct();
            Assert.Throws<InvalidOperationException>(() => product.DesactivateProduct());
        }
    }
}
