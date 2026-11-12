using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Products.Queries.GetProductById
{
    public class GetProductByIdHandler
    {
        private readonly IProductQueries _productQueries;

        public GetProductByIdHandler(IProductQueries productQueries)
        {
            _productQueries = productQueries;
        }

        public async Task<ProductDetailsDto> Handle(GetProductByIdQuery query, CancellationToken ct)
        {
            return await _productQueries.GetByIdAsync(query.Id, ct) 
                ?? throw new NotFoundException(nameof(Product), query.Id);
        }
    }
}
