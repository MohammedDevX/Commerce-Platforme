namespace Catalogue.Application.ProductImage.Create
{
    public record UploadImageCommand(
        Guid ProductId, 
        Stream Content, 
        string FileName, 
        string ContentType, 
        long Size)
    {}
}
