using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Commands.Create;
using Catalogue.Domaine.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using System.Runtime.InteropServices;

namespace Catalogue.Tests.Application.Products.Create
{
    public class CreateProductHandlerTest
    {
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<ICompanyRepository> _companyRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTest()
        {
            _handler = new CreateProductHandler(_companyRepository.Object, 
                _productRepository.Object, 
                _unitOfWork.Object);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Company_doesnt_Exists()
        {
            var companyId = Guid.NewGuid();
            var command = new CreateProductCommand(companyId, "Razor");
            _companyRepository
                .Setup(c => c.ExistsAsync(companyId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _productRepository.Verify(p => p.Add(It.IsAny<Product>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Company_Exists_And_AddProduct_And_SaveIt()
        {
            var companyId = Guid.NewGuid();
            string companyName = "Razor";
            var command = new CreateProductCommand(companyId, companyName);

            _companyRepository
                .Setup(c => c.ExistsAsync(companyId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            await _handler.Handle(command, CancellationToken.None);

            _productRepository
                .Verify(p => p.Add(It.Is<Product>(p => p.CompanyId == companyId && p.Name == companyName)),
                Times.Once);

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
