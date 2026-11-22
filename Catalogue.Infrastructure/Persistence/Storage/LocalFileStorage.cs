using Catalogue.Application.Common.Abstractions.Storage;
using Catalogue.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Catalogue.Infrastructure.Persistence.Storage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly ImageStorageOptions _imageStorageOptions;
        private readonly string _UploadedFolderPath = null!;

        public LocalFileStorage(IOptions<ImageStorageOptions> options)
        {
            _imageStorageOptions = options.Value;
            Path.Combine(Directory.GetCurrentDirectory(), _imageStorageOptions.RootPath);
            Directory.CreateDirectory(_UploadedFolderPath!);
        }

        // The uploads folder should be outside of the executable directory, and inject the path in ENV

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
