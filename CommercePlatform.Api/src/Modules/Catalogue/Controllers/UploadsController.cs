using Catalogue.Application.Products.Commands.ProductImage.AddImage;
using CommercePlatform.Api.src.Dtos.Requests.Uploads;
using Microsoft.AspNetCore.Mvc;

namespace CommercePlatform.Api.src.Modules.Catalogue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly AddProductImageHandler _uploadImageHandler;

        public UploadsController(AddProductImageHandler uploadImageHandler)
        {
            _uploadImageHandler = uploadImageHandler;
        }


        [HttpPost("/{¨ProductId}/images")]
        public async Task<ActionResult> UploadImage(
            [FromForm] UploadImageRequestDto request,
            CancellationToken ct)
        {
            await using Stream content = request.File.OpenReadStream();
            AddProductImageCommand command = new(
                request.ProductId,
                content, request.File.FileName,
                request.File.ContentType,
                request.File.Length
            );

            await _uploadImageHandler.Handle(command, ct);

            //return CreatedAtAction()
            return Ok();
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
    }
}
