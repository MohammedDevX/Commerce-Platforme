namespace Catalogue.Infrastructure.Options
{
    public sealed class ImageStorageOptions
    {
        public const string SectionName = "LocalImageStorage";

        public string RootPath { get; set; } = null!;
    }
}
