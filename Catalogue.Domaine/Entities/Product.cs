using Catalogue.Domaine.Entities.Exceptions;
using Catalogue.Domaine.Entities.ValueObjects;
using Catalogue.Domaine.Enums;

namespace Catalogue.Domaine.Entities
{
    public class Product
    {
        private const int MaxImages = 5;
        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = null!;
        public Guid BrandId { get; private set; }
        public ProductStatus Status { get; private set; } = ProductStatus.Activate;
        public Company Company { get; set; }
        private readonly List<ProductVariant> _variants = new();
        // This syntax is an explisite {get => return _variants;}
        /* 
         * And .AsReadOnly() wrap the original collection into ReadOnlyCollection so if someone outside of
         * the class try to modify in the collection an InvalidCastException is thrown
         */
        public IReadOnlyCollection<ProductVariant> ProductVariants => _variants.AsReadOnly();
        private readonly List<ProductImage> _images = new();
        public IReadOnlyCollection<ProductImage> ProductImages => _images.AsReadOnly();

        public Product()
        {
        }

        public Product(Guid companyId, string productName, string description, Guid brandId) 
        {
            if (companyId == Guid.Empty)
                throw new ArgumentException("CompanyId cannot by empty");

            if (brandId == Guid.Empty)
                throw new ArgumentException("BrandId cannot by empty");

            Rename(productName);
            AddDescription(description);

            Id = Guid.NewGuid();
            CompanyId = companyId;
            BrandId = brandId;
        }

        public void Rename(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product Name cannot be empty, null or contain only spaces");

            Name = productName.Trim();
        }

        private void AddDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty, null or contain only spaces");

            Description = description;
        }

        public void ActivateProduct()
        {
            if (Status is ProductStatus.Inactivate)
            {
                Status = ProductStatus.Activate;
                return;
            }

            throw new InvalidOperationException("The product is already activated");
        }

        public void DesactivateProduct()
        {
            if (Status is ProductStatus.Activate)
            {
                Status = ProductStatus.Inactivate;
                return;
            }

            throw new InvalidOperationException("The product is already inactive");
        }

        public void AddProductVariant(Money price, Sku sku)
        {
            if (Status is ProductStatus.Inactivate)
                throw new InvalidOperationException("Cannot add variant to an inactive product");

            ProductVariant productVariant = new(Id, price, sku);
            _variants.Add(productVariant);
        }

        public void AddImage(string fileKey, string type, long size)
        {
            var imagesCount = _images.Count();
            if (imagesCount > MaxImages)
                throw new InvalidOperationException("Product cannot have more than 5 images");

            var isPrimaryImageExists = _images.Any(i => i.IsPrimary == true);
            bool isPrimary = false;
            if (!isPrimaryImageExists)
            {
                isPrimary = true;
            }

            var productImage = ProductImage.Create(Id, fileKey, type, isPrimary, size);
            _images.Add(productImage);
        }

        public void SetPrimaryImage(Guid imageId)
        {
            var image = _images.SingleOrDefault(i => i.Id == imageId) ??
                throw new NotFoundException(nameof(ProductImage), imageId);
                
            if (image.IsPrimary)
                return;

            foreach (var item in _images)
            {
                item.UnmarkAsPrimary();
            }

            image.MarkAsPrimary();
        }
    }
}
