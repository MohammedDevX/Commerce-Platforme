using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;
using Catalogue.Infrastructure.Persistence.Repositories;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;
using Catalogue.Infrastructure.Persistence;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Catalogue.Tests.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// IClassFixture<T> => whene xUnit framework find a test class that implemente IClassFixture<T>
    /// he create an instance of T fixture class and injecte it in the constructor of the test class 
    /// </summary>
    public class ProductVariantRepositoryTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixture;

        public ProductVariantRepositoryTest(CatalogueDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        /*
            Note : If the work flow reaches RollbackAsync() so the rollback is triggred, but if the 
            work flow end with throwing an exception before reaches RollbackAsync() so here, because 
            of using {} block if there is no Commit() so Dispose() is going to trigger a Rollback
         */
        [Fact]
        public async Task Check_If_Sku_Already_Exists()
        {
            var company = new Company("Razor");
            var product = new Product(company.Id, "Razor Mouse 101");
            var sku = new Sku("razor-mouse-101");
            var money = new Money(140m);
            CancellationToken ct = CancellationToken.None;

            await using var context = _fixture.CreateDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync();
            product.AddProductVariant(money, sku);
            context.Companies.Add(company);
            context.Products.Add(product);
            await context.SaveChangesAsync(ct);
            ProductVariantRepository _productVariantRepository = new ProductVariantRepository(context);
            var isSkuExists = await _productVariantRepository.ExistsSkuForProductAsync(sku.SKU, product.Id, ct);
            Assert.True(isSkuExists);
            await transaction.RollbackAsync(ct);
        }

        [Fact]
        public async Task Check_If_Sku_Doesnt_Exists()
        {
            CancellationToken ct = CancellationToken.None;
            await using var context = _fixture.CreateDbContext();
            var company = new Company("Logitech");
            var product = new Product(company.Id, "Logitech Z-1000");
            var sku = new Sku("Logitech-Z-1000");
            var money = new Money(150m);
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            context.Companies.Add(company);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            ProductVariantRepository productVariantRepository = new(context);
            var isSkuExists = await productVariantRepository.ExistsSkuForProductAsync(sku.SKU, product.Id, ct);

            Assert.False(isSkuExists);

            await transaction.RollbackAsync(ct);
        }

        [Fact]
        public async Task Check_If_Sku_Alredy_Exists_In_Other_Poruct()
        {
            CancellationToken ct = CancellationToken.None;
            await using var context = _fixture.CreateDbContext();
            var company = new Company("Logitech");
            var product = new Product(company.Id, "Logitech Z-1000 mouse");
            var product2 = new Product(company.Id, "Logitech12 keyboard");
            var sku = new Sku("Logitech-Z-1000");
            var money = new Money(150m);

            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            product.AddProductVariant(money, sku);
            context.Companies.Add(company);
            context.Products.Add(product);
            context.Products.Add(product2);
            await context.SaveChangesAsync(ct);

            ProductVariantRepository productVariantRepository = new(context);
            var isSkuExists = await productVariantRepository.ExistsSkuForProductAsync(sku.SKU, product2.Id, ct);

            Assert.False(isSkuExists);

            await transaction.RollbackAsync(ct);
        }
    }
}
