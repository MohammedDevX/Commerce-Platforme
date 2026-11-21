using Catalogue.Application.Common.Abstractions.Storage;
using Microsoft.Extensions.Configuration;

namespace Catalogue.Infrastructure.Persistence.Storage
{
    public class LocalFileStorage : IFileStorage
    {
        // The uploads folder should be outside of the executable directory, and inject the path in ENV
        private readonly string _UploadedFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public LocalFileStorage()
        {
            Directory.CreateDirectory(_UploadedFolderPath);
        }

        public async Task SaveAsync(Stream content, string fileName, string fileKey, CancellationToken ct = default)
        {
            string uploadImagePath = Path.Combine(_UploadedFolderPath, fileKey);
            await using FileStream dest = new(uploadImagePath, FileMode.Create, FileAccess.Write);
            await content.CopyToAsync(dest, ct);
        }

        public async Task DeleteAsync(string fileName)
        {
            // Delete image from local
        }
    }
}
