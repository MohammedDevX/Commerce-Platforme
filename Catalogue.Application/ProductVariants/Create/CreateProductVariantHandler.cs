using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Application.ProductVariants.Create
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
            // throw NotFoundException if product doesnt existe after installing EF !!!
            var product = await _productRepository.GetProductByIdAsync(command.ProductId, ct) ?? throw new NotFoundException(nameof(Product), command.ProductId);

            var variantExisting = await _productVariantRepository.ExistsSkuForCompanyAsync(command.Sku, product.CompanyId, ct);
            if (variantExisting)
                throw new ConflictException($"Variant with SKU {command.Sku} already existe in company with id {product.CompanyId}");

            product.AddProductVariant(new Money(command.Price), command.Sku);

            // Save the variant in DB
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
