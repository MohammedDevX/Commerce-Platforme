using Catalogue.Application.Abstractions.Persistence;
using Catalogue.Domaine.Entities.ValueObjects;

namespace Catalogue.Application.ProductVariants.Create
{
    public class CreateProductVariantHandler(
        IProductRepository productRepositiry, 
        ICompanyRepository companyRepository,
        IProductVariantRepository productVariantRepository)
    {
        private readonly IProductRepository _productRepository = productRepositiry;
        private readonly ICompanyRepository _companyRepository = companyRepository;
        private readonly IProductVariantRepository _productVariantRepository = productVariantRepository;

        public async Task Handle(CreateProductVariantCommand command, CancellationToken ct)
        {
            // throw NotFoundException if product doesnt existe after installing EF !!!
            var product = await _productRepository.GetProductByIdAsync(command.ProductId, ct); //?? throw new NotFoundException();

            await _companyRepository.GetCompanyByIdAsync(product.CompanyId, ct); // throw new NotFoundException();

            var variantExisting = await _productVariantRepository.ExistsSkuForCompanyAsync(command.Sku, ct);
            if (variantExisting == false)
                // throw Conflict exception !!!

            product.AddProductVariant(new Money(command.Price), command.Sku);

            // Save the variant in DB
        }
    }
}
