namespace Catalogue.Domaine.Entities
{
    public class ProductImage
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string FileKey { get; private set; } = null!;
        public string ContentType { get; private set; } = null!;
        public long Size { get; private set; }
        public bool IsPrimary { get; private set; }
        public Product Product { get; private set; } = null!;

        public ProductImage()
        {}

        public static ProductImage Create(Guid productId, string fileKey, string type, bool isPrimary, long size)
        {
            if(productId == Guid.Empty)
                throw new ArgumentException("ProductId is required");

            if (string.IsNullOrEmpty(fileKey.Trim()))
                throw new ArgumentException("File key is required");

            return new ProductImage()
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                FileKey = fileKey,
                ContentType = type,
                Size = size,
                IsPrimary = isPrimary
            };
        }

        internal void MarkAsPrimary() => IsPrimary = true;
        internal void UnmarkAsPrimary() => IsPrimary = false;

    }
}
