
using Catalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Catalogue.Tests.Infrastructure.Fixtures
{
    /// <summary>
    /// Fixtures is a class that centralise operations repeatable in many test cases
    /// </summary>

    /// <summary>
    /// And the interface IAsyncLifetime resolve probleme of running an async operation before
    /// tests cases so the first solution you think about to run operation in first is using constructor
    /// but the ctor cannot be async so thats meen the thread is not released, thats why IAsyncLifetime
    /// fix this probleme by overriding InitializeAsync() => runned before tests cases and its async
    /// </summary>
    public class CatalogueDatabaseFixture : IAsyncLifetime
    {
        private CatalogueDbContext context;
        private IDbContextTransaction transaction;

    public CatalogueDatabaseFixture()
        {
            context = CreateDbContext();   
        }

        public async Task DisposeAsync()
        {
            
        }

        public async Task InitializeAsync()
        {
            CancellationToken ct = CancellationToken.None;
            var context = CreateDbContext();
            await context.Database.MigrateAsync(ct);
            transaction = await context.Database.BeginTransactionAsync();
        }

        public CatalogueDbContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .Build();

            string connectionString = configuration.GetConnectionString("catalogueConnectionString")!;

            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
             .UseSqlServer(connectionString)
                .Options;

            return new CatalogueDbContext(options);
        }
    }
}
