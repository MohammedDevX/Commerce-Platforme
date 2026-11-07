using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Create;
using Catalogue.Domaine.Entities;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Repositories;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace Catalogue.Tests.Integration.Products
{
    public class CreateProductHandlerIntegrationTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixture;

        public CreateProductHandlerIntegrationTest(CatalogueDatabaseFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public async Task Add_Product_In_Database_Integration_Test()
        {
            CancellationToken ct = CancellationToken.None;
            await using var context = _fixture.CreateDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            IProductRepository productRepository = new ProductRepository(context);
            ICompanyRepository companyRepository = new CompanyRepository(context);
            IUnitOfWork unitOfWork = new UnitOfWork(context);
            CreateProductHandler hander = new(companyRepository, productRepository, unitOfWork);

            string companyName = "Razor";
            string productName = "Razor-Mouse 101";
            var company = new Company(companyName);
            CreateProductCommand command = new(company.Id, productName);

            context.Companies.Add(company);
            await context.SaveChangesAsync(ct);

            await hander.Handle(command, ct);

            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
                .UseSqlServer(context.Database.GetDbConnection()).Options;
            var context2 = new CatalogueDbContext(options);
            await context2.Database.UseTransactionAsync(transaction.GetDbTransaction());

            var product = await context2.Products.FirstOrDefaultAsync(p => p.CompanyId == company.Id);

            Assert.NotNull(product);
            Assert.Equal(productName, product.Name);

            await transaction.CommitAsync(ct);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Company_Doenst_Exits()
        {
            CancellationToken ct = CancellationToken.None;
            await using var context = _fixture.CreateDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync(ct);

            IProductRepository productRepository = new ProductRepository(context);
            ICompanyRepository companyRepository = new CompanyRepository(context);
            IUnitOfWork unitOfWork = new UnitOfWork(context);
            CreateProductHandler hander = new(companyRepository, productRepository, unitOfWork);

            string productName = "Razor-Mouse 101";
            Guid companyId = Guid.NewGuid();
            CreateProductCommand command = new(companyId, productName);

            await Assert.ThrowsAsync<NotFoundException>(() => hander.Handle(command, ct));

            var product = await context.Products.FirstOrDefaultAsync(p => p.CompanyId == companyId);

            Assert.Null(product);

            await transaction.RollbackAsync(ct);
        }
    }
}
