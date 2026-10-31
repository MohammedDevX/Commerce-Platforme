using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Tests.Domaine.Entities
{
    public class ProductVariantTest
    {
        [Fact]
        public void ProductVariant_Constructor_Validate_Test()
        {
            Money money = new Money(12m);
            Sku sku = new Sku("RAZOR-MOUSE-101");
            ProductVariant productVariant = new(Guid.NewGuid(), money, sku);
            Assert.Equal(12m, productVariant.Price.Price);
            Assert.Equal("RAZOR-MOUSE-101", productVariant.SKU.SKU);
        }

        [Fact]
        public void Should_Throw_Exception_If_ProducId_Is_Empty_Test()
        {
            Money money = new Money(12m);
            Sku sku = new Sku("RAZOR-MOUSE-101");
            Guid productId = Guid.Empty;
            Action action = () => new ProductVariant(productId, money, sku);
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Should_Throw_Exception_If_Price_Is_Null_Test()
        {
            Money money = null;
            Sku sku = new Sku("RAZOR-MOUSE-101");
            Guid productId = Guid.NewGuid();
            Action action = () => new ProductVariant(productId, money, sku);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Should_Sku_Be_Normalized_Test()
        {
            Money money = new Money(12m);
            Sku sku = new Sku("razor-mouse-101    ");
            ProductVariant productVariant = new(Guid.NewGuid(), money, sku);
            Assert.Equal("RAZOR-MOUSE-101", productVariant.SKU.SKU);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void Should_Throw_Exception_If_Sku_Is_Null_Or_Is_Empty_Or_Contain_Only_Spaces_Test(string sku)
        {
            Money money = new Money(12m);
            Assert.Throws<ArgumentException>(() => new ProductVariant(Guid.NewGuid(), money, new Sku(sku)));
        }
    }
}
