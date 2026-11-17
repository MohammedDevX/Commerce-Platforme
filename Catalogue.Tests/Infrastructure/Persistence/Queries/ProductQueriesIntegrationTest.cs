using Catalogue.Domaine.Entities;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Queries;
using Catalogue.Tests.Common.Builders;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalogue.Tests.Infrastructure.Persistence.Queries
{
    public class ProductQueriesIntegrationTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixture;

        public ProductQueriesIntegrationTest(CatalogueDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetProductById_Test()
        {
            var contexts = await _fixture.CreateSharedTransactionAsync();
            var context = contexts.context1;
            var context2 = contexts.context2;
            var transaction = contexts.transaction;
            CancellationToken ct = CancellationToken.None;

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
            var contexts = await _fixture.CreateSharedTransactionAsync();
            var context = contexts.context1;
            var context2 = contexts.context2;
            var transaction = contexts.transaction;
            CancellationToken ct = CancellationToken.None;

            var productQueries = new ProductQueries(context2);

            var res = await productQueries.GetByIdAsync(Guid.NewGuid(), ct);

            Assert.Null(res);

            await transaction.RollbackAsync(ct);
        }

        [Fact]
        public async Task GetAllAsync_Test()
        {
            var contexts = await _fixture.CreateSharedTransactionAsync();
            var context = contexts.context1;
            var context2 = contexts.context2;
            var transaction = contexts.transaction;
            CancellationToken ct = CancellationToken.None;

            var company = new CompanyBuilder().Build();
            var p1 = new ProductBuilder().WithCompanyId(company.Id).Build();
            var p2 = new ProductBuilder().WithCompanyId(company.Id).WithName("Hp Z-102").Build();

            var queries = new ProductQueries(context2);

            context.Companies.Add(company);
            context.Products.AddRange(p1, p2);
            await context.SaveChangesAsync(ct);

            var result = await queries.GetAllAsync(10, 1, ct);

            Assert.NotEmpty(result);
            Assert.Equal(4, result.Count);

            await transaction.RollbackAsync(ct);
        }

        [Fact]
        public async Task GetProductsCount_Test()
        {
            var contexts = await _fixture.CreateSharedTransactionAsync();
            var context = contexts.context1;
            var context2 = contexts.context2;
            var transaction = contexts.transaction;
            CancellationToken ct = CancellationToken.None;

            var company = new CompanyBuilder().Build();
            var p1 = new ProductBuilder().WithCompanyId(company.Id).Build();
            var p2 = new ProductBuilder().WithCompanyId(company.Id).WithName("Hp Z-102").Build();

            var queries = new ProductQueries(context2);

            context.Companies.Add(company);
            context.Products.AddRange(p1, p2);
            await context.SaveChangesAsync(ct);

            var result = await queries.GetProductsCountAsync(ct);

            Assert.Equal(2, result);

            await transaction.RollbackAsync(ct);
        }
    }
}
