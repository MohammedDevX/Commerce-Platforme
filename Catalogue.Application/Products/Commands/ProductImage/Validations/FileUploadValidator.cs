using Catalogue.Application.Common.Options;
using Microsoft.Extensions.Options;

namespace Catalogue.Application.Products.Commands.ProductImage.Validations
{
    internal class FileUploadValidator
    {
        private readonly ImageStorageRulesOptions _settings;
        private static string[] _AllowedExtensions = null!;
        private static long MaxSize;

        public FileUploadValidator(IOptions<ImageStorageRulesOptions> options)
        {
            _settings = options.Value;
            _AllowedExtensions = _settings.AllowedExtensions;
            MaxSize = _settings.MaxSize;
        }

        public static void Validate(string fileKey, string type, long size)
        {
            if (!_AllowedExtensions.Contains(type))
                throw new ArgumentException("Image content type is not valid");

            if (size == 0)
                throw new ArgumentException("Image cannot be empty");

            if (size >= MaxSize)
                throw new ArgumentException("The max size of image is 1mb");

            // Can add more validations
        }
    }
}
