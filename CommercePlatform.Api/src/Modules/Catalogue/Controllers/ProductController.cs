using Catalogue.Application.Common.Models;
using Catalogue.Application.ProductImage.Create;
using Catalogue.Application.Products.Queries.GetProducts;
using Catalogue.Infrastructure.Persistence;
using CommercePlatform.Api.src.Dtos;
using Microsoft.AspNetCore.Mvc;


namespace CommercePlatform.Api.src.Modules.Catalogue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GetProductsHandler _getProductsHandler;

        public ProductController(GetProductsHandler getProductsHandler, UploadImageHandler uploadImageHandler)
        {
            _getProductsHandler = getProductsHandler;
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<ProductListItemDto>>> GetProductsAsync(
            [FromQuery] GetProductsQuery query, 
            CancellationToken ct)
        {
            var result = await _getProductsHandler.Handle(query, ct);
            return Ok(result);
        }
    }
}
