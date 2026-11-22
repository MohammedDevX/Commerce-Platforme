using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Abstractions.Storage;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Common.Generators;
using Catalogue.Application.Products.Commands.ProductImage.Validations;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Products.Commands.ProductImage.AddImage
{
    public class AddProductImageHandler
    {
        private readonly IFileStorage _fileStorage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public AddProductImageHandler(
            IFileStorage fileStorage, 
            IUnitOfWork unitOfWork, 
            IProductRepository productRepository)
        {
            _fileStorage = fileStorage;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task Handle(AddProductImageCommand command, CancellationToken ct)
        {
            var product = await _productRepository.GetProductByIdAsync(command.ProductId, ct);

            if (product is null)
                throw new NotFoundException(nameof(Product), command.ProductId);

            string fileKey = FileKeyGenerator.Generate(product.Id, command.FileName);
            FileUploadValidator.Validate(fileKey, command.ContentType, command.Size);

            product.AddImage(fileKey, command.ContentType, command.Size);
            
            await _fileStorage.SaveAsync(command.Content, command.FileName, fileKey, ct);

            try
            {
                await _unitOfWork.SaveChangesAsync(ct);
            } catch (Exception ex)
            {
                // Delete the uploaded image from Disk after implementing DeleteAsync()
                throw;
            }
        }
    }
}
