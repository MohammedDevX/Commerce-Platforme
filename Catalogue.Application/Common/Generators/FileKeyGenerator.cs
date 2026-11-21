namespace Catalogue.Application.Common.Generators
{
    internal class FileKeyGenerator
    {
        public static string Generate(string fileName)
        {
            // Add the upload directory path in ENV
            string fileUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(fileUploadPath);
            string uploadedFileExtension = Path.GetExtension(fileName);
            string fileKey = Path.Combine($@"{fileUploadPath}\{Guid.NewGuid()}{uploadedFileExtension}");
            return fileKey;
        }
    }
}
