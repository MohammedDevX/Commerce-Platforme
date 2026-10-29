using Catalogue.Application.Abstractions.Persistence;

namespace Catalogue.Application.ProductVariants.Create
{
    public class CreateProductVariantHandler(IProductRepository productRepositiry, ICompanyRepository companyRepository)
    {
        private readonly IProductRepository _productRepository = productRepositiry;
        private readonly ICompanyRepository _companyRepository = companyRepository;

        public async Task Handle(CreateProductVariantCommand command, CancellationToken ct)
        {
            // throw NotFoundException if product doesnt existe after installing EF !!!
            var product = await _productRepository.GetProductByIdAsync(command.ProductId, ct); //?? throw new NotFoundException();

            await _companyRepository.GetCompanyByIdAsync(product.CompanyId, ct); // throw new NotFoundException();

            product.AddProductVariant(command.Price, command.Sku);

            // Save the variant in DB
        }
    }
}
