using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Desactivate;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Products.Commands.Activate
{
    public class ActivateProductHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateProductHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ActivateProductCommand command, CancellationToken ct)
        {
            var product = await _productRepository.GetProductByIdAsync(command.Id, ct)
                ?? throw new NotFoundException(nameof(Product), command.Id);

            product.ActivateProduct();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
