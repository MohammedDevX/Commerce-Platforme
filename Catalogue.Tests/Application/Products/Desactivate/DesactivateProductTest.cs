using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Desactivate;
using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Enums;
using Moq;

namespace Catalogue.Tests.Application.Products.Desactivate
{
    public class DesactivateProductTest
    {
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly DesactivateProductHandler _desactivateProductHandler;

        public DesactivateProductTest()
        {
            _desactivateProductHandler = new DesactivateProductHandler(
                _productRepository.Object,
                _unitOfWork.Object);
        }


        [Fact]
        public async Task DesactivateProductHandler_Test()
        {
            var product = new Product(Guid.NewGuid(), "Razor");
            var command = new DesactivateProductCommand(product.Id);

            _productRepository
                .Setup((pr) => pr.GetProductByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            await _desactivateProductHandler.Handle(command, CancellationToken.None);

            Assert.Equal(ProductStatus.Desactivate, product.Status);

            _unitOfWork.Verify((u) => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Product_NotFound_And_Dont_DesactivateProduct()
        {
            var product = new Product(Guid.NewGuid(), "Razor Mouse");
            var command = new DesactivateProductCommand(product.Id);

            _productRepository
                .Setup((pr) => pr.GetProductByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _desactivateProductHandler.Handle(command, CancellationToken.None));

            Assert.Equal(ProductStatus.Activate, product.Status);

            _unitOfWork.Verify((u) => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
