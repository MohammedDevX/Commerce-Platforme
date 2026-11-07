using Catalogue.Domaine.Entities;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Repositories;
using Catalogue.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalogue.Tests.Infrastructure.Persistence.Repositories
{
    public class CompanyRepositoryTest : IClassFixture<CatalogueDatabaseFixture>
    {
        private readonly CatalogueDatabaseFixture _fixture;

        public CompanyRepositoryTest(CatalogueDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Verify_If_Company_Exists()
        {
            await using var context = _fixture.CreateDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var company = new Company("Microsoft");

            context.Companies.Add(company);
            await context.SaveChangesAsync();

            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
                .UseSqlServer(context.Database.GetDbConnection()).Options;
            await using var context2 = new CatalogueDbContext(options);
            await context2.Database.UseTransactionAsync(transaction.GetDbTransaction());

            var companyRepository = new CompanyRepository(context2);
            var existsCompany = await companyRepository.ExistsAsync(company.Id, CancellationToken.None);

            Assert.True(existsCompany);

            await transaction.RollbackAsync();
        }

        [Fact]
        public async Task Verify_If_Company_Doesnt_Exists()
        {
            await using var context = _fixture.CreateDbContext();

            var companyRepository = new CompanyRepository(context);
            var existsCompany = await companyRepository.ExistsAsync(Guid.NewGuid(), CancellationToken.None);

            Assert.False(existsCompany);
        }
    }
}
