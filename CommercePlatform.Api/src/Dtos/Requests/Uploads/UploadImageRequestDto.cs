namespace CommercePlatform.Api.src.Dtos.Requests.Uploads
{
    public record UploadImageRequestDto(IFormFile File, Guid ProductId)
    {
    }
}
