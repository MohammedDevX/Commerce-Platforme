using Catalogue.Domaine.Entities;

namespace Catalogue.Tests
{
    public class CompanyTest
    {
        [Fact]
        public void Company_Name_Validation_Test()
        {
            string name = "Facebook";
            Company company = new(name);
            Assert.Equal("Facebook", company.Name);
        }

        [Fact]
        public void Should_Company_Name_Doesnt_Contain_Spaces_Test()
        {
            string name = " Facebook ";
            Company company = new(name);
            Assert.Equal("Facebook", company.Name);
        }

        //[Fact]
        //public void Should_Throw_Exception_If_Company_Name_Is_Empty_Test()
        //{
        //    string name = "";
        //    var exception = Assert.Throws<ArgumentException>(() => new Company(name));
        //    Assert.Equal("The name cannot be empty", exception.Message);
        //}

        //[Fact]
        //public void Should_Throw_Exception_If_Company_Name_Is_Null_Test()
        //{
        //    string name = null;
        //    var exception = Assert.Throws<ArgumentException>(() => new Company(name));
        //    Assert.Equal("The name cannot be empty", exception.Message);
        //}

        [Theory]
        [InlineData("    ")]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Throw_Exception_If_Company_Name_Contain_Only_Spaces_Or_Is_Empty_Or_Is_Null_Test(string name)
        {
            var exception = Assert.Throws<ArgumentException>(() => new Company(name));
        }
    }
}
