namespace Catalogue.Application.Products.Commands.ProductImage.AddImage
{
    public record AddProductImageCommand(
        Guid ProductId, 
        Stream Content, 
        string FileName, 
        string ContentType, 
        long Size)
    {}
}
