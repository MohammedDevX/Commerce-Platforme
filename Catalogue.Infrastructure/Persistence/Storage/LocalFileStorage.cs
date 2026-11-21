using Catalogue.Application.Common.Abstractions.Storage;
using Microsoft.Extensions.Configuration;

namespace Catalogue.Infrastructure.Persistence.Storage
{
    public class LocalFileStorage : IFileStorage
    {
        public async Task<string> SaveAsync(Stream content, string fileName, string fileKey, CancellationToken ct = default)
        {
            FileStream dest = new(fileKey, FileMode.Create, FileAccess.Write);
            await content.CopyToAsync(dest);
            return fileName;
        }

        public async Task DeleteAsync(string fileName)
        {
            // Delete image from local
        }
    }
}
