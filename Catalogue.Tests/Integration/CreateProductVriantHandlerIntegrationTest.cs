using Castle.DynamicProxy.Generators;
using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.ProductVariants.Create;
using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;
using Catalogue.Infrastructure.Persistence.Repositories;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.Tests.Integration
{
    public class CreateProductVriantHandlerIntegrationTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixture;

        public CreateProductVriantHandlerIntegrationTest(CatalogueDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Add_ProductVariant_In_Database_Integration_Test()
        {
            await using var context = _fixture.CreateDbContext();

            CancellationToken ct = CancellationToken.None;
            var company = new Company("Razor");
            var produit = new Product(company.Id, "Razor mouse");

            context.Companies.Add(company);
            context.Products.Add(produit);
            await context.SaveChangesAsync(ct);

            var money = new Money(140m);
            var sku = new Sku("RAZOR-101-MOUSE");
            var createProductVariantCommand = new CreateProductVariantCommand
                (produit.Id, money.Price, sku.SKU);

            var handler = new CreateProductVariantHandler(
                new ProductRepository(context),
                new ProductVariantRepository(context),
                new UnitOfWork(context)
                );

            await handler.Handle(createProductVariantCommand, ct);

            // Get the Saved product to check if persistence work sucessfully

            await using var context2 = _fixture.CreateDbContext();
            var saved = await new ProductRepository(context2)
                .GetProductByIdAsync(createProductVariantCommand.ProductId, ct);

            var product = await context2.Products
                .Where(p => p.Id == createProductVariantCommand.ProductId)
                .Include(p => p.ProductVariants).FirstOrDefaultAsync(ct);
            var variant = new ProductVariantRepository(context2);

            Assert.Single(product!.ProductVariants);
            Assert.True(await variant.ExistsSkuForProductAsync(sku.SKU, createProductVariantCommand.ProductId, ct));
        }
    }
}
