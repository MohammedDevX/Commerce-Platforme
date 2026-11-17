using Castle.DynamicProxy.Generators;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Queries.GetProductById;
using Catalogue.Domaine.Entities;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Queries;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Transactions;

namespace Catalogue.Tests.Integration.Products.Queries
{
    public class GetProductByIdHandlerInegrationTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixture;

        public GetProductByIdHandlerInegrationTest(CatalogueDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetProducByIdHandler_Intergation_Test()
        {
            var contexts = await _fixture.CreateSharedTransactionAsync();
            var context = contexts.context1;
            var context2 = contexts.context2;
            var transaction = contexts.transaction;
            CancellationToken ct = CancellationToken.None;

            var company = new Company("Razor");
            var product = new Product(company.Id, "Razor Mouse");

            var query = new GetProductByIdQuery(product.Id);
            var handler = new GetProductByIdHandler(new ProductQueries(context2));

            context.Companies.Add(company);
            context.Products.Add(product);
            await context.SaveChangesAsync(ct);

            ProductDetailsDto dto = await handler.Handle(query, ct);

            Assert.NotNull(dto);
            Assert.Equal(product.Id, dto.Id);
            Assert.Equal(product.CompanyId, dto.CompanyId);
            Assert.Equal(product.Name, dto.Name);
            Assert.Equal(product.Status.ToString(), dto.Status);

            await transaction.RollbackAsync(ct);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Product_NotFound_GetProducByIdHandler_Intergation_Test()
        {
            var contexts = await _fixture.CreateSharedTransactionAsync();
            var context = contexts.context1;
            var context2 = contexts.context2;
            var transaction = contexts.transaction;
            CancellationToken ct = CancellationToken.None;
            var query = new GetProductByIdQuery(Guid.NewGuid());
            var handler = new GetProductByIdHandler(new ProductQueries(context2));

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, ct));

            await transaction.RollbackAsync(ct);
        }
    }
}
