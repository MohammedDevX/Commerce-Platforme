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
        private readonly UploadImageHandler _uploadImageHandler;

        public ProductController(GetProductsHandler getProductsHandler, UploadImageHandler uploadImageHandler)
        {
            _getProductsHandler = getProductsHandler;
            _uploadImageHandler = uploadImageHandler;
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<ProductListItemDto>>> GetProductsAsync(
            [FromQuery] GetProductsQuery query, 
            CancellationToken ct)
        {
            var result = await _getProductsHandler.Handle(query, ct);

            return Ok(result);
        }

        //[HttpPost]
        //public async Task<ActionResult> UploadImage(IFormFile image)
        //{
        //    string imagesDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
        //    Directory.CreateDirectory(imagesDirectoryPath);
        //    string imageName = Path.GetFileName(image.FileName);
        //    string imageExtension = Path.GetExtension(imageName).ToLowerInvariant();
        //    string imagePath = Path.Combine($@"{imagesDirectoryPath}\{Guid.NewGuid()}{imageExtension}");
        //    using var stream = new FileStream(imagePath, FileMode.Create, FileAccess.Write);
        //    await image.CopyToAsync(stream);
        //    return Ok(new { stream.Name, stream.Length});
        //}

        [HttpPost]
        public async Task<ActionResult> UploadImage(
            [FromForm] UploadImageRequestDto request, 
            CancellationToken ct)
        {
            Stream content = request.File.OpenReadStream();
            UploadImageCommand command = new(
                request.ProductId, 
                content, request.File.FileName, 
                request.File.ContentType, 
                request.File.Length
            );

            await _uploadImageHandler.Handle(command, ct);

            return Ok();
        }
    }
}
