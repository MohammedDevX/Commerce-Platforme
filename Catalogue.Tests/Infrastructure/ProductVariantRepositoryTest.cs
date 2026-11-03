using Catalogue.Domaine.Entities;
using Catalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Catalogue.Tests.Infrastructure
{
    public class ProductVariantRepositoryTest
    {
        // This method create CatalogueDbContext object each time is called
        // Note : It use test env connection string 
        private CatalogueDbContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.Test.json")
                .Build();

            string connectionString = configuration.GetConnectionString("catalogueConnectionString")!;

            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
             .UseSqlServer(connectionString)
                .Options;

            return new CatalogueDbContext(options);
        }
    }
}
