namespace Catalogue.Application.Common.Options
{
    public sealed class ImageStorageRulesOptions
    {
        public const string SectionName = "StorageRules";

        public string[] AllowedExtensions { get; set; } = null!;
        public long MaxSize { get; set; }
    }
}
