using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Application.Products.Queries.GetProductById;
using Catalogue.Domaine.Entities;
using Moq;

namespace Catalogue.Tests.Application.Products.Queries
{
    public class GetProductByIdTest
    {
        private readonly Mock<IProductQueries> _productQuery = new();
        private readonly GetProductByIdHandler _handler;

        public GetProductByIdTest()
        {
            _handler = new GetProductByIdHandler(_productQuery.Object);
        }

        [Fact]
        public async Task GetProductByIdHandler_Test()
        {
            var company = new Company("Razore");
            var product = new Product(company.Id, "Razor Mouse");
            var dto = new ProductDetailsDto(product.Id, company.Id, product.Name, product.Status.ToString());

            var query = new GetProductByIdQuery(product.Id);
            _productQuery
                .Setup((pq) => pq.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            var res = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(product.Id, res.Id);
            Assert.Equal(product.Name, res.Name);
            Assert.Equal(product.CompanyId, res.CompanyId);
            Assert.Equal(product.Status.ToString(), res.Status);
        }

        [Fact]
        public async Task Throw_NotFound_Exception_If_ProductNotFound_GetProductByIdHandler()
        {
            var query = new GetProductByIdQuery(Guid.NewGuid());
            _productQuery
                .Setup((pq) => pq.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDetailsDto)null!);

            await Assert
                .ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));


        }
    }
}
