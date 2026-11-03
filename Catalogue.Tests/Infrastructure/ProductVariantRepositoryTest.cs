using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

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
                .AddJsonFile("appsettings.Test.json")
                .Build();

            string connectionString = configuration.GetConnectionString("catalogueConnectionString")!;

            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
             .UseSqlServer(connectionString)
                .Options;

            return new CatalogueDbContext(options);
        }

        [Fact]
        public async Task Check_If_Sku_Already_Exists()
        {

            CancellationToken ct = CancellationToken.None;
            using var context = CreateDbContext();
            await context.Database.MigrateAsync(ct);
            var company = new Company("Razor");
                var product = new Product(company.Id, "Razor Mouse 101");
                var sku = new Sku("razor-mouse-101");
                var money = new Money(140m);

            product.AddProductVariant(money, sku);

                context.Companies.Add(company);
                context.Products.Add(product);
                await context.SaveChangesAsync(ct);

            using var context2 = CreateDbContext();
            ProductVariantRepository _productVariantRepository = new ProductVariantRepository(context2);
            var isSkuExists = await _productVariantRepository.ExistsSkuForProductAsync(sku.SKU, product.Id, ct);

            Assert.True(isSkuExists);
        }

        [Fact]
        public async Task Check_If_Sku_Doesnt_Exists()
        {
            CancellationToken ct = CancellationToken.None;
            using var context = CreateDbContext();
            var company = new Company("Logitech");
            var product = new Product(company.Id, "Logitech Z-1000");
            var sku = new Sku("Logitech-Z-1000");
            var money = new Money(150m);

            context.Companies.Add(company);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            using var context2 = CreateDbContext();
            ProductVariantRepository productVariantRepository = new(context2);
            var isSkuExists = await productVariantRepository.ExistsSkuForProductAsync(sku.SKU, product.Id, ct);

            Assert.False(isSkuExists);
        }

        [Fact]
        public async Task Check_If_Sku_Alredy_Exists_In_Other_Poruct()
        {
            CancellationToken ct = CancellationToken.None;
            using var context = CreateDbContext();
            var company = new Company("Logitech");
            var product = new Product(company.Id, "Logitech Z-1000 mouse");
            var product2 = new Product(company.Id, "Logitech12 keyboard");
            var sku = new Sku("Logitech-Z-1000");
            var money = new Money(150m);

            product.AddProductVariant(money, sku);
            context.Companies.Add(company);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            using var context2 = CreateDbContext();
            ProductVariantRepository productVariantRepository = new(context2);
            var isSkuExists = await productVariantRepository.ExistsSkuForProductAsync(sku.SKU, product2.Id, ct);

            Assert.False(isSkuExists);
        }
    }
}
