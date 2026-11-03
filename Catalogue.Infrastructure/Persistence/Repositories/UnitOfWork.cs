using Catalogue.Application.Common.Abstractions.Persistence;

namespace Catalogue.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogueDbContext _context;

        public UnitOfWork(CatalogueDbContext context)
        {
            _context = context;
        }

        // UnitOfWork should get the current CatalogueDbContext using in requests fulent to share the same
        // Change tracker
        public Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return _context.SaveChangesAsync(ct);
        }
    }
}