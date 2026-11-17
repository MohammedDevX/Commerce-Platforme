using Catalogue.Application.Products.Queries.GetProducts;
using Catalogue.Domaine.Entities;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Queries;
using Catalogue.Tests.Common.Builders;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Catalogue.Tests.Integration.Products.Queries
{
    public class GetProductsHandlerIntegrationTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixture;

        public GetProductsHandlerIntegrationTest(CatalogueDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetProductsHandler_Test()
        {
            var contexts = await _fixture.CreateSharedTransactionAsync();
            var context = contexts.context1;
            var context2 = contexts.context2;
            var transaction = contexts.transaction;
            CancellationToken ct = CancellationToken.None;

            var company = new CompanyBuilder().Build();
            var products = new[]
            {
                new ProductBuilder().WithCompanyId(company.Id).Build(),   // "Razor Mouse"
                new ProductBuilder().WithCompanyId(company.Id).WithName("HP ELIT BOOK").Build(),
                new ProductBuilder().WithCompanyId(company.Id).WithName("HP ELIT BOOK PRO").Build(),
                new ProductBuilder().WithCompanyId(company.Id).WithName("ASUS PRO").Build(),
                new ProductBuilder().WithCompanyId(company.Id).WithName("MSI PREDATOR").Build(),
                new ProductBuilder().WithCompanyId(company.Id).WithName("LOGITECH 101").Build(),
            };
            var handler = new GetProductsHandler(new ProductQueries(context2));
            var query = new GetProductsQuery(5, 1);

            context.Companies.Add(company);
            context.Products.AddRange(products);
            await context.SaveChangesAsync(ct);

            var result = await handler.Handle(query, ct);

            Assert.NotEmpty(result.items);
            Assert.Equal(6, result.totalCount);
            Assert.Equal(5, result.TotalItems);
            Assert.True(result.HasNext);
            Assert.False(result.HasPrevious);
            Assert.Equal(2, result.TotalPages);

            await transaction.RollbackAsync(ct);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_PageNumber_NotValid()
        {
            await using var context = _fixture.CreateDbContext();
            var ct = CancellationToken.None;
            var handler = new GetProductsHandler(new ProductQueries(context));
            var query = new GetProductsQuery(5, 0);

            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(query, ct));
        }

        [Theory]
        [InlineData(101)]
        [InlineData(0)]
        public async Task Should_Throw_Exception_If_PageSize_NotValid(int pageSize)
        {
            await using var context = _fixture.CreateDbContext();
            var ct = CancellationToken.None;
            var handler = new GetProductsHandler(new ProductQueries(context));
            var query = new GetProductsQuery(pageSize, 5);

            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(query, ct));
        }

        [Fact]
        public async Task GetProductsHandler_Filter_By_Name_Test()
        {
            var contexts = await _fixture.CreateSharedTransactionAsync();
            var context = contexts.context1;
            var context2 = contexts.context2;
            var transaction = contexts.transaction;
            CancellationToken ct = CancellationToken.None;
            var handler = new GetProductsHandler(new ProductQueries(context2));
            var query = new GetProductsQuery(5, 1);
            var company = new CompanyBuilder().Build();
            var products = new[]
            {
                new ProductBuilder().WithCompanyId(company.Id).Build(),   // "Razor Mouse"
                new ProductBuilder().WithCompanyId(company.Id).WithName("HP ELIT BOOK").Build(),
                new ProductBuilder().WithCompanyId(company.Id).WithName("HP ELIT BOOK PRO").Build(),
                new ProductBuilder().WithCompanyId(company.Id).WithName("ASUS PRO").Build(),
                new ProductBuilder().WithCompanyId(company.Id).WithName("MSI PREDATOR").Build(),
                new ProductBuilder().WithCompanyId(company.Id).WithName("LOGITECH 101").Build(),
            };

            context.Companies.Add(company);

            context.Products.AddRange(products);
            await context.SaveChangesAsync(ct);

            var result = await handler.Handle(query, ct);

            Assert.Equal(
                new[] { "ASUS PRO", "HP ELIT BOOK", "HP ELIT BOOK PRO", "LOGITECH 101", "MSI PREDATOR" },
                result.items.Select(i => i.Name)
            );

            await transaction.RollbackAsync(ct);
        }
    }
}
