using Catalogue.Domaine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Tests
{
    public class CompanyTest
    {
        [Fact]
        public void Company_Name_Validation_Test()
        {
            string name = "Facebook";
            Company company = new(name);
            Assert.Equal("FACEBOOK", company.Name);
        }

        [Fact]
        public void Should_Company_Name_Doesnt_Contain_Spaces_Test()
        {
            string name = " Facebook ";
            Company company = new(name);
            Assert.Equal("FACEBOOK", company.Name);
        }

        [Fact]
        public void Should_Throw_Exception_If_Company_Name_Is_Empty_Test()
        {
            string name = "";
            var exception = Assert.Throws<ArgumentException>(() => new Company(name));
            Assert.Equal("The name cannot be empty", exception.Message);
        }

        [Fact]
        public void Should_Throw_Exception_If_Company_Name_Is_Null_Test()
        {
            string name = null;
            var exception = Assert.Throws<ArgumentException>(() => new Company(name));
            Assert.Equal("The name cannot be empty", exception.Message);
        }

        [Fact]
        public void Should_Throw_Exception_If_Company_Name_Contain_Only_Spaces_Test()
        {
            string name = "     ";
            var exception = Assert.Throws<ArgumentException>(() => new Company(name));
            Assert.Equal("The name cannot be empty", exception.Message);
        }
    }
}
