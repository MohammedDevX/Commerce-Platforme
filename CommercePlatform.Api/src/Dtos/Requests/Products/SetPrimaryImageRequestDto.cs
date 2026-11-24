namespace CommercePlatform.Api.src.Dtos.Requests.Products
{
    public sealed record SetPrimaryImageRequestDto(Guid ProductId, Guid ImageId)
    {
    }
}
