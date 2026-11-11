using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Activate;
using Catalogue.Application.Products.Desactivate;
using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Enums;
using Moq;

namespace Catalogue.Tests.Application.Products.Activate
{
    public class ActivateProductTest
    {
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly ActivateProductHandler _activateProductHandler;

        public ActivateProductTest() => _activateProductHandler = new ActivateProductHandler(
            _productRepository.Object,
            _unitOfWork.Object);

        [Fact]
        public async Task ActivateProductHandler_Test()
        {
            var product = new Product(Guid.NewGuid(), "Razor");
            product.DesactivateProduct();
            var command = new ActivateProductCommand(product.Id);

            _productRepository
                .Setup((pr) => pr.GetProductByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            await _activateProductHandler.Handle(command, CancellationToken.None);

            Assert.Equal(ProductStatus.Activate, product.Status);

            _unitOfWork.Verify((u) => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Product_NotFound_And_Dont_ActivateProduct()
        {
            var product = new Product(Guid.NewGuid(), "Razor Mouse");
            product.DesactivateProduct();
            var command = new ActivateProductCommand(product.Id);

            _productRepository
                .Setup((pr) => pr.GetProductByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _activateProductHandler.Handle(command, CancellationToken.None));

            Assert.Equal(ProductStatus.Desactivate, product.Status);

            _unitOfWork.Verify((u) => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
