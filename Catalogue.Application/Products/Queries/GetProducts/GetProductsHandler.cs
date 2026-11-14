using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Common.Models;

namespace Catalogue.Application.Products.Queries.GetProducts
{
    public class GetProductsHandler
    {
        private readonly IProductQueries _productQueries;

        public GetProductsHandler(IProductQueries productQueries)
        {
            _productQueries = productQueries;
        }

        public async Task<PageResult<ProductListItemDto>> Handle(GetProductsQuery query, CancellationToken ct)
        {
            var result = await _productQueries.GetAllAsync(query.pageSize, query.pageNumber, ct);
            var resultCount = await _productQueries.GetProductsCountAsync(ct);
            return new PageResult<ProductListItemDto>(result, resultCount, query.pageNumber, query.pageSize);
        }
    }
}
