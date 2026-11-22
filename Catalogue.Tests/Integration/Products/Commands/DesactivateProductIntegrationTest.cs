using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Commands.Desactivate;
using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Enums;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Repositories;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalogue.Tests.Integration.Products.Commands
{
    public class DesactivateProductIntegrationTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixture;

        public DesactivateProductIntegrationTest(CatalogueDatabaseFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task DesactivateProductHandler_Integration_Test()
        {
            await using var context = _fixture.CreateDbContext();
            var ct = CancellationToken.None;
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            var company = new Company("Razor");
            var product = new Product(company.Id, "Razor Mouse");
            var command = new DesactivateProductCommand(product.Id);
            var handler = new DesactivateProductHandler(
                new ProductRepository(context), 
                new UnitOfWork(context)
            );

            context.Companies.Add(company);
            context.Products.Add(product);

            await context.SaveChangesAsync(ct);

            await handler.Handle(command, ct);

            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
                .UseSqlServer(context.Database.GetDbConnection())
                .Options;

            await using var context2 = new CatalogueDbContext(options);
            await context2.Database.UseTransactionAsync(transaction.GetDbTransaction(), ct);

            var desactivatedProduct = await context2.Products
                .FirstOrDefaultAsync(p => p.Id == product.Id, ct);

            Assert.NotNull(desactivatedProduct);
            Assert.Equal(ProductStatus.Inactivate, desactivatedProduct.Status);

            await transaction.RollbackAsync(ct);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Product_NotFound_And_DontDesactivateProduct_Test()
        {
            await using var context = _fixture.CreateDbContext();
            var ct = CancellationToken.None;
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            var command = new DesactivateProductCommand(Guid.NewGuid());
            var handler = new DesactivateProductHandler(
                new ProductRepository(context),
                new UnitOfWork(context)
            );

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, ct));

            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
                .UseSqlServer(context.Database.GetDbConnection())
                .Options;

            await using var context2 = new CatalogueDbContext(options);
            await context2.Database.UseTransactionAsync(transaction.GetDbTransaction(), ct);

            var desactivatedProduct = await context2.Products.FirstOrDefaultAsync(p => p.Id == command.Id, ct);

            Assert.Null(desactivatedProduct);

            await transaction.RollbackAsync(ct);
        }
    }
}
