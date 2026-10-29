namespace Catalogue.Application.Common.Abstractions.Persistence
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
