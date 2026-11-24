namespace CommercePlatform.Api.src.Dtos.Requests.Products
{
    public sealed record CreateProductRequestDto(Guid CompanyId, string Name, string Description, Guid BrandId)
    {
    }
}
