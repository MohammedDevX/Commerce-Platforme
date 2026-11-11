using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Application.ProductVariants.Commands.Create
{
    public class CreateProductVariantHandler(
        IProductRepository productRepositiry, 
        IProductVariantRepository productVariantRepository,
        IUnitOfWork unitOfWork)
    {
        private readonly IProductRepository _productRepository = productRepositiry;
        private readonly IProductVariantRepository _productVariantRepository = productVariantRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(CreateProductVariantCommand command, CancellationToken ct)
        {
            Money price = new Money(command.Price);
            Sku sku = new Sku(command.Sku);

            // Here we fetched the product from DB without using AsNoTracking() because the product should 
            // be tracked because he is the responsible of adding the variant in DB
            var product = await _productRepository.GetProductByIdAsync(command.ProductId, ct) 
                ?? throw new NotFoundException(nameof(Product), command.ProductId);

            var variantExisting = await _productVariantRepository.
                ExistsSkuForProductAsync(sku.SKU, product.Id, ct);

            if (variantExisting)
                throw new ConflictException
                    ($"Variant with SKU {command.Sku} already existe in product with id {product.Id}");

            product.AddProductVariant(price, sku);

            // Save the variant in DB
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
