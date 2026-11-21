namespace CommercePlatform.Api.src.Dtos
{
    public record UploadImageRequestDto(IFormFile File, Guid ProductId)
    {
    }
}
