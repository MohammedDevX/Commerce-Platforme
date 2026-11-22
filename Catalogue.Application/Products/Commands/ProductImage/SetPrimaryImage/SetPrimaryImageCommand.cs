namespace Catalogue.Application.Products.Commands.ProductImage.SetPrimaryImage
{
    public record SetPrimaryImageCommand(Guid ProductId, Guid ProductImageId)
    {
    }
}
