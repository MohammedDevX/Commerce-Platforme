using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Products.Commands.Create
{
    public class CreateProductHandler
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductHandler(ICompanyRepository companyRepository, 
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateProductResponseDto> Handle(CreateProductCommand commande, CancellationToken ct)
        {
            var isCompnyExists = await _companyRepository.ExistsAsync(commande.companyId, ct);
            if (isCompnyExists == false)
                throw new NotFoundException(nameof(Company), commande.companyId);

            var product = new Product(commande.companyId, commande.name, commande.description, commande.brandId);
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateProductResponseDto(product.Id, 
                product.CompanyId, 
                product.Name, 
                product.Status.ToString(), 
                product.Description, 
                product.BrandId);
        }
    }
}
