using Catalogue.Application.Products.Queries.GetProducts;
using Catalogue.Infrastructure.Persistence.Queries;
using Catalogue.Tests.Common.Builders;
using Catalogue.Tests.Infrastructure.Fixtures;
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
            await using var context = _fixture.CreateDbContext();
            await using var context2 = _fixture.CreateDbContext();
            var ct = CancellationToken.None;
            var company = new CompanyBuilder().Build();
            var p1 = new ProductBuilder().WithCompanyId(company.Id).Build();
            var p2 = new ProductBuilder().WithCompanyId(company.Id).WithName("HP ELIT BOOK").Build();
            var p3 = new ProductBuilder().WithCompanyId(company.Id).WithName("HP ELIT BOOK PRO").Build();
            var p4 = new ProductBuilder().WithCompanyId(company.Id).WithName("ASUS PRO").Build();
            var p5 = new ProductBuilder().WithCompanyId(company.Id).WithName("MSI PREDATOR").Build();
            var p6 = new ProductBuilder().WithCompanyId(company.Id).WithName("LOGITECH 101").Build();
            var handler = new GetProductsHandler(new ProductQueries(context2));
            var query = new GetProductsQuery(5, 1);

            context.Companies.Add(company);
            context.Products.AddRange(p1, p2, p3, p4, p5, p6);
            await context.SaveChangesAsync(ct);

            var result = await handler.Handle(query, ct);

            Assert.NotEmpty(result.items);
            Assert.Equal(6, result.totalCount);
            Assert.Equal(5, result.TotalItems);
            Assert.True(result.HasNext);
            Assert.False(result.HasPrevious);
            Assert.Equal(2, result.TotalPages);
        }

        [Fact]
        public async Task ShouldThrow_Exception_If_GetProductsQury_Data_NotValid()
        {
            await using var context = _fixture.CreateDbContext();
            var ct = CancellationToken.None;
            var handler = new GetProductsHandler(new ProductQueries(context));
            var query = new GetProductsQuery(0, 101);

            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(query, ct));
        }
    }
}
