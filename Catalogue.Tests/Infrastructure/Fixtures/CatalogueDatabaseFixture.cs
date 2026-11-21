
using Catalogue.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Testcontainers.MsSql;

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
        private MsSqlContainer _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Your_password123!")
            .Build();

        //public CatalogueDatabaseFixture()
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.Test.json")
        //        .Build();
        //}

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            await using var context = CreateDbContext();
            await context.Database.MigrateAsync();
        }

        /// <summary>
        /// This method create CatalogueDbContext object each time is called
        /// Note : It use test env connection string 
        /// </summary>
        public CatalogueDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
             .UseSqlServer(_container.GetConnectionString())
                .Options;

            return new CatalogueDbContext(options);
        }

        public async Task<(CatalogueDbContext context1, 
            CatalogueDbContext context2, 
            IDbContextTransaction transaction)> CreateSharedTransactionAsync(CancellationToken ct = default)
        {
            var context = CreateDbContext();
            var transaction = await context.Database.BeginTransactionAsync(ct);

            var options = new DbContextOptionsBuilder<CatalogueDbContext>()
                .UseSqlServer(context.Database.GetDbConnection())
                .Options;

            var context2 = new CatalogueDbContext(options);
            await context2.Database.UseTransactionAsync(transaction.GetDbTransaction(), ct);

            return (context, context2, transaction);
        }
    }
}
