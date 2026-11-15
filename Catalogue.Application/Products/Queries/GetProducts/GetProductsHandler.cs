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
            if (query.pageNumber <= 0 || query.pageSize <= 0 || query.pageSize > 100)
                throw new ArgumentException("Page size or page number are not valid");

            var result = await _productQueries.GetAllAsync(query.pageSize, query.pageNumber, ct);
            var resultCount = await _productQueries.GetProductsCountAsync(ct);
            return new PageResult<ProductListItemDto>(result, resultCount, query.pageNumber, query.pageSize);
        }
    }
}
