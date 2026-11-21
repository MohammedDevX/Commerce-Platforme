namespace Catalogue.Application.Common.Generators
{
    internal class FileKeyGenerator
    {
        public static string Generate(Guid producId, string fileName)
        {
            string uploadedFileExtension = Path.GetExtension(fileName);
            string fileKey = Path.Combine($@"product/{producId}/{Guid.NewGuid()}{uploadedFileExtension}");
            return fileKey;
        }
    }
}
