using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Products.Update
{
    public class UpdateProductHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProductCommand command, CancellationToken ct)
        {
            var product = await _productRepository.GetProductByIdAsync(command.Id, ct) 
                ?? throw new NotFoundException(nameof(Product), command.Id);

            product.Rename(command.Name);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
