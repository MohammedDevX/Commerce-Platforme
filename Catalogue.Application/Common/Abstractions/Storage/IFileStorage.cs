namespace Catalogue.Application.Common.Abstractions.Storage
{
    public interface IFileStorage
    {
        public Task SaveAsync(Stream content, string fileName, string fileKey, CancellationToken ct);
        public Task DeleteAsync(string fileName);
    }
}
