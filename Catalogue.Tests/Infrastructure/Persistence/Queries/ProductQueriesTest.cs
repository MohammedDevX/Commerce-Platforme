using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Queries;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Catalogue.Tests.Infrastructure.Persistence.Queries
{
    public class ProductQueriesTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixtuer;

        public ProductQueriesTest(CatalogueDatabaseFixture fixture)
        {
            _fixtuer = fixture;
        }

        [Fact]
        public async Task GetProductById_Test()
        {
            await using var context = _fixtuer.CreateDbContext();
            var ct = CancellationToken.None;
            await using var transaction = await context.Database.BeginTransactionAsync(ct);
            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
                .UseSqlServer(context.Database.GetDbConnection())
                .Options;
            await using var context2 = new CatalogueDbContext(options);
            await context2.Database.UseTransactionAsync(transaction.GetDbTransaction(), ct);

            var company = new Company("Razor");
            var product = new Product(company.Id, "Razor Mouse");

            var productQueries = new ProductQueries(context2);

            context.Companies.Add(company);
            context.Products.Add(product);

            await context.SaveChangesAsync(ct);

            var res = await productQueries.GetByIdAsync(product.Id, ct);

            Assert.NotNull(res);
            Assert.Equal(product.Id, res.Id);
            Assert.Equal(product.CompanyId, res.CompanyId);
            Assert.Equal(product.Name, res.Name);
            Assert.Equal(product.Status.ToString(), res.Status);

            await transaction.RollbackAsync(ct);
        }

        [Fact]
        public async Task Should_Return_Null_If_Product_NotFound_GetProductById_Test()
        {
            await using var context = _fixtuer.CreateDbContext();
            var ct = CancellationToken.None;
            await using var transaction = await context.Database.BeginTransactionAsync(ct);
            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
                .UseSqlServer(context.Database.GetDbConnection())
                .Options;
            await using var context2 = new CatalogueDbContext(options);
            await context2.Database.UseTransactionAsync(transaction.GetDbTransaction(), ct);

            var productQueries = new ProductQueries(context2);

            var res = await productQueries.GetByIdAsync(Guid.NewGuid(), ct);

            Assert.Null(res);

            await transaction.RollbackAsync(ct);
        }
    }
}
