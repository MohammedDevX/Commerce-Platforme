using Catalogue.Application.Common.Models;
using Catalogue.Application.Products.Commands.Activate;
using Catalogue.Application.Products.Commands.Create;
using Catalogue.Application.Products.Commands.Desactivate;
using Catalogue.Application.Products.Commands.ProductImage.AddImage;
using Catalogue.Application.Products.Commands.ProductImage.SetPrimaryImage;
using Catalogue.Application.Products.Commands.Update;
using Catalogue.Application.Products.Queries.GetProductById;
using Catalogue.Application.Products.Queries.GetProducts;
using CommercePlatform.Api.src.Dtos.Requests.Products;
using Microsoft.AspNetCore.Mvc;


namespace CommercePlatform.Api.src.Modules.Catalogue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GetProductsHandler _getProductsHandler;
        private readonly SetPrimaryImageHandler _setPrimaryImageHandler;
        private readonly GetProductByIdHandler _getProductByIdHandler;
        private readonly CreateProductHandler _createProductHandler;
        private readonly UpdateProductHandler _updateProductHandler;
        private readonly DesactivateProductHandler _desactivateProductHandler;
        private readonly ActivateProductHandler _activateProductHandler;

        public ProductController(
            GetProductsHandler getProductsHandler,
            AddProductImageHandler uploadImageHandler,
            SetPrimaryImageHandler setPrimaryImageHandler,
            GetProductByIdHandler getProductByIdHandler,
            CreateProductHandler createProductHandler,
            UpdateProductHandler updateProductHandler,
            DesactivateProductHandler desactivateProductHandler,
            ActivateProductHandler activateProductHandler)
        {
            _getProductsHandler = getProductsHandler;
            _setPrimaryImageHandler = setPrimaryImageHandler;
            _getProductByIdHandler = getProductByIdHandler;
            _createProductHandler = createProductHandler;
            _updateProductHandler = updateProductHandler;
            _desactivateProductHandler = desactivateProductHandler;
            _activateProductHandler = activateProductHandler;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailsDto>> GetProductByIdAsync(Guid id, CancellationToken ct)
        {
            GetProductByIdQuery query = new(id);
            var response = await _getProductByIdHandler.Handle(query, ct);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<ProductListItemDto>>> GetProductsAsync(
            [FromQuery] GetProductsQuery query,
            CancellationToken ct)
        {
            var result = await _getProductsHandler.Handle(query, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CreateProductResponseDto>> CreateProductAsync(CreateProductRequestDto dto, CancellationToken ct)
        {
            var command = new CreateProductCommand(dto.CompanyId, dto.Name, dto.Description, dto.BrandId);
            var response = await _createProductHandler.Handle(command, ct);

            return CreatedAtAction(nameof(GetProductByIdAsync), new { response.Id }, response);
        }

        [HttpPatch("{ProductId}")]
        public async Task<ActionResult> SetPrimaryImage(SetPrimaryImageRequestDto dto, CancellationToken ct)
        {
            var command = new SetPrimaryImageCommand(dto.ProductId, dto.ImageId);
            await _setPrimaryImageHandler.Handle(command, ct);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(UpdateProductCommand command, CancellationToken ct)
        {
            await _updateProductHandler.Handle(command, ct);

            return NoContent();
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult> DesactivateProduct(DesactivateProductCommand command, CancellationToken ct)
        {
            await _desactivateProductHandler.Handle(command, ct);

            return NoContent();
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult> ActivateProduct(ActivateProductCommand command, CancellationToken ct)
        {
            await _activateProductHandler.Handle(command, ct);

            return NoContent();
        }
    }
}
