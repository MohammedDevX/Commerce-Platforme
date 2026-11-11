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

        public async Task Handle(CreateProductCommand commande, CancellationToken ct)
        {
            var isCompnyExists = await _companyRepository.ExistsAsync(commande.companyId, ct);
            if (isCompnyExists == false)
                throw new NotFoundException(nameof(Company), commande.companyId);

            var product = new Product(commande.companyId, commande.name);
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
