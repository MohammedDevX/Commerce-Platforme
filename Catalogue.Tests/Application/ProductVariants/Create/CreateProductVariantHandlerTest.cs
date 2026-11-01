using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.ProductVariants.Create;
using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;
using Moq;

namespace Catalogue.Tests.Application.ProductVariants.Create
{
    public class CreateProductVariantHandlerTest
    {
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<IProductVariantRepository> _productVariantRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CreateProductVariantHandler _handler;
        public CreateProductVariantHandlerTest()
        {
            _handler = new CreateProductVariantHandler(_productRepository.Object, _productVariantRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task CreateProductVariant_Handle_Async()
        {
            Company company = new("Razor");
            Product product = new(company.Id, "Razor Mouse");
            Sku sku = new("razor_mouse_101");
            Money price = new(150m);
            CreateProductVariantCommand command = new(product.Id, price.Price, sku.SKU);

            _productRepository.Setup(p => p.GetProductByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            _productVariantRepository.Setup(v => v.ExistsSkuForCompanyAsync(sku.SKU, company.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            await _handler.Handle(command, CancellationToken.None);

            Assert.Single(product.ProductVariants);
            Assert.Equal("RAZOR_MOUSE_101", product.ProductVariants.First().SKU.SKU);
            Assert.Equal(price.Price, product.ProductVariants.First().Price.Price);

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Exception_And_Dont_Add_Variant_If_Product_Doesnt_ExisteAsync()
        {
            Company company = new("Razor");
            Sku sku = new("razor_mouse_101");
            Money price = new(150m);
            CreateProductVariantCommand command = new(Guid.NewGuid(), price.Price, sku.SKU);

            _productRepository.Setup(p => p.GetProductByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);
            _productVariantRepository.Setup(v => v.ExistsSkuForCompanyAsync(sku.SKU, company.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Throw_Exception_And_Dont_Add_Variant_If_Sku_Already_Existe_Async()
        {
            Company company = new("Razor");
            Product product = new(company.Id, "Razor Mouse");
            Sku sku = new("razor_mouse_101");
            Money price = new(150m);
            CreateProductVariantCommand command = new(product.Id, price.Price, sku.SKU);

            _productRepository.Setup(p => p.GetProductByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            _productVariantRepository.Setup(v => v.ExistsSkuForCompanyAsync(sku.SKU, company.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(() => _handler.Handle(command, CancellationToken.None));

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Throw_Exception_And_Dont_Add_Variant_If_Price_Is_Invalid_Async()
        {
            Company company = new("Razor");
            Product product = new(company.Id, "Razor Mouse");
            Sku sku = new("razor_mouse_101");
            CreateProductVariantCommand command = new(product.Id, -150m, sku.SKU);

            _productRepository.Setup(p => p.GetProductByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            _productVariantRepository.Setup(v => v.ExistsSkuForCompanyAsync(sku.SKU, company.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Throw_Exception_And_Dont_Add_Variant_If_Sku_Is_Invalid_Async()
        {
            Company company = new("Razor");
            Product product = new(company.Id, "Razor Mouse");
            Money price = new(140m);
            string sku = "razor*;:²  _mouse_101";
            CreateProductVariantCommand command = new(product.Id, price.Price, sku);

            _productRepository.Setup(p => p.GetProductByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            _productVariantRepository.Setup(v => v.ExistsSkuForCompanyAsync(sku, company.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
