namespace Catalogue.Domaine.Entities
{
    public class ProductImage
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string FileKey { get; private set; } = null!;
        public bool IsPrimary { get; private set; }
        public Product Product { get; private set; } = null!;

        public ProductImage()
        {}

        public static ProductImage Create(Guid productId, string fileKey, string type, bool isPrimary, long size)
        {
            if(productId == Guid.Empty)
                throw new ArgumentException($"{nameof(productId)} is required");

            return new ProductImage()
            {
                Id = new Guid(),
                ProductId = productId,
                FileKey = fileKey,
                IsPrimary = isPrimary
            };
        }
    }
}
