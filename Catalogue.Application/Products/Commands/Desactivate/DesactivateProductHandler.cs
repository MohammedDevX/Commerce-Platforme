using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Products.Commands.Desactivate
{
    public class DesactivateProductHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DesactivateProductHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DesactivateProductCommand command, CancellationToken ct)
        {
            var product = await _productRepository.GetProductByIdAsync(command.Id, ct) 
                ?? throw new NotFoundException(nameof(Product), command.Id);

            product.DesactivateProduct();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
