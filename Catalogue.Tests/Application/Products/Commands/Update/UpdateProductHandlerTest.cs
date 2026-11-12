using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Commands.Update;
using Catalogue.Domaine.Entities;
using Moq;

namespace Catalogue.Tests.Application.Products.Commands.Update
{
    public class UpdateProductHandlerTest
    {
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly UpdateProductHandler _updateProductHandler;

        public UpdateProductHandlerTest()
        {
            _updateProductHandler = new UpdateProductHandler(_productRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_Product_Exists_UpdateIt_And_SaveIt()
        {
            var product = new Product(Guid.NewGuid(), "Razor Mouse 101");
            string updatedName = "Razor Mouse 201";
            var command = new UpdateProductCommand(product.Id, updatedName);

            _productRepository
                .Setup(p => p.GetProductByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            await _updateProductHandler.Handle(command, CancellationToken.None);

            Assert.Equal(updatedName, product.Name);

            _unitOfWork.Verify((u) => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Product_DoesntExists_Should_Throw_Exception()
        {
            string updatedName = "Razor Mouse 201";
            var command = new UpdateProductCommand(Guid.NewGuid(), updatedName);

            _productRepository
                .Setup(p => p.GetProductByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            await Assert
                .ThrowsAsync<NotFoundException>(() => _updateProductHandler.Handle(command, CancellationToken.None));

            _unitOfWork.Verify((u) => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
