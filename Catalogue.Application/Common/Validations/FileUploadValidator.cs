namespace Catalogue.Application.Common.Validations
{
    internal class FileUploadValidator
    {
        private static string[] _ValidateFormats = ["image/jpeg", "image/webp"];
        private const long MaxSize = 1000000;
        public static void Validate(string fileKey, string type, long size)
        {
            if (string.IsNullOrEmpty(fileKey.Trim()))
                throw new ArgumentException($"{nameof(fileKey)} is required");

            if (!_ValidateFormats.Contains(type))
                throw new ArgumentException("Image content type is not valid");

            if (size > MaxSize)
                throw new ArgumentException("The max size of image is 1mb");
        }
    }
}
