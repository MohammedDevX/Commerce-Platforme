using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Abstractions.Storage;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Common.Generators;
using Catalogue.Application.Common.Validations;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.ProductImage.Create
{
    public class UploadImageHandler
    {
        private readonly IFileStorage _fileStorage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public UploadImageHandler(
            IFileStorage fileStorage, 
            IUnitOfWork unitOfWork, 
            IProductRepository productRepository)
        {
            _fileStorage = fileStorage;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task Handle(UploadImageCommand command, CancellationToken ct)
        {
            var product = await _productRepository.GetProductByIdAsync(command.ProductId, ct);

            if (product is null)
                throw new NotFoundException(nameof(Product), command.ProductId);

            string fileKey = FileKeyGenerator.Generate(command.FileName);
            FileUploadValidator.Validate(fileKey, command.ContentType, command.Size);

            string fileName = await _fileStorage.SaveAsync(command.Content, command.FileName, fileKey, ct);

            product.AddImage(fileKey, command.ContentType, command.Size);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
