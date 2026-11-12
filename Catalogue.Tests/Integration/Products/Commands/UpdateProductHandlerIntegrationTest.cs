using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Commands.Update;
using Catalogue.Domaine.Entities;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Repositories;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Runtime.InteropServices;

namespace Catalogue.Tests.Integration.Products.Commands
{
    public class UpdateProductHandlerIntegrationTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixture;

        public UpdateProductHandlerIntegrationTest(CatalogueDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Update_Product_Handler_Test()
        {
            var ct = CancellationToken.None;
            await using var context = _fixture.CreateDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync(ct);
            var company = new Company("Razor");
            string oldName = "Razor Mouse 101";
            string updatedName = "Razor Mouse 201";
            var product = new Product(company.Id, oldName);
            var command = new UpdateProductCommand(product.Id, updatedName);
            var handler = new UpdateProductHandler(new ProductRepository(context), new UnitOfWork(context));

            context.Companies.Add(company);
            context.Products.Add(product);
            await context.SaveChangesAsync(ct);

            await handler.Handle(command, ct);

            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
                .UseSqlServer(context.Database.GetDbConnection()).Options;

            await using var context2 = new CatalogueDbContext(options);
            await context2.Database.UseTransactionAsync(transaction.GetDbTransaction());

            var updatedProduct =  await new ProductRepository(context2).GetProductByIdAsync(product.Id, ct);

            Assert.NotNull(updatedProduct);
            Assert.Equal(updatedName, updatedProduct.Name);

            await transaction.RollbackAsync(ct);
        }

        [Fact]
        public async Task Should_UpdateProductHandler_Throw_Exception_If_ProductNotFound()
        {
            await using var context = _fixture.CreateDbContext();
            var ct = CancellationToken.None;
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            var company = new Company("Razor");
            string updatedName = "Razor Mouse 201";
            var command = new UpdateProductCommand(Guid.NewGuid(), updatedName);
            var handler = new UpdateProductHandler(new ProductRepository(context), new UnitOfWork(context));

            context.Companies.Add(company);
            await context.SaveChangesAsync(ct);

            await Assert.ThrowsAnyAsync<NotFoundException>(() => handler.Handle(command, ct));

            await transaction.RollbackAsync(ct);
        }
    }
}
