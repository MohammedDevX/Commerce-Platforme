namespace Catalogue.Application.ProductImage.Validations
{
    internal class FileUploadValidator
    {
        private static string[] _AllowedExtensions = ["image/jpeg", "image/webp"];
        private const long MaxSize = 1000000; // 1mb
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
