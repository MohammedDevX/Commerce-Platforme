
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
        private readonly string _connectionString;

        public CatalogueDatabaseFixture()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .Build();

            _connectionString = configuration.GetConnectionString("catalogueConnectionString")
                ?? throw new InvalidOperationException("ConnectionString configuration not found");
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
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
             .UseSqlServer(_connectionString)
                .Options;

            return new CatalogueDbContext(options);
        }
    }
}
