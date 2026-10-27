using Catalogue.Domaine.Entities;

namespace Catalogue.Tests
{
    public class ProductTest
    {
        [Fact]
        public void Constructor_Validate_Test()
        {
            Guid id = Guid.NewGuid();
            Product product = new(id, "Razor mouse");
            Assert.Equal(id, id);
            Assert.Equal("Razor mouse", product.Name);
        }

        [Fact]
        public void Should_PoroductName_Be_Normalized_Test()
        {
            string productName = " Razor Mouse ";
            Assert.Equal("Razor mouse", new Product(Guid.NewGuid(), productName).Name);
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
    }
}
