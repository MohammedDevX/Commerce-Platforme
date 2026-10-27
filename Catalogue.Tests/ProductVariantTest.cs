using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Tests
{
    public class ProductVariantTest
    {
        [Fact]
        public void ProductVariant_Constructor_Validate_Test()
        {
            ProductVariant productVariant = new(Guid.NewGuid(), new Money(12m), "RAZOR-MOUSE-101");
            Assert.Equal(12m, productVariant.Price.Price);
            Assert.Equal("RAZOR-MOUSE-101", productVariant.Sku);
        }

        [Fact]
        public void Should_Throw_Exception_If_ProducId_Is_Empty_Test()
        {
            Assert.Throws<ArgumentException>(() => new ProductVariant(Guid.Empty, new Money(12m), "RAZOR-MOUSE-101"));
        }

        [Fact]
        public void Should_Throw_Exception_If_Price_Is_Null_Test()
        {
            Assert.Throws<ArgumentException>(() => new ProductVariant(Guid.NewGuid(), null, "RAZOR-MOUSE-101"));
        }

        [Fact]
        public void Should_Sku_Be_Normalized_Test()
        {
            ProductVariant productVariant = new(Guid.NewGuid(), new Money(140m), "razor-mouse-101  ");
            Assert.Equal("RAZOR-MOUSE-101", productVariant.Sku);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void Should_Throw_Exception_If_Sku_Is_Null_Or_Is_Empty_Or_Contain_Only_Spaces_Test(string sku)
        {
            Assert.Throws<ArgumentException>(() => new ProductVariant(Guid.NewGuid(), new Money(140m), sku));
        }
    }
}
