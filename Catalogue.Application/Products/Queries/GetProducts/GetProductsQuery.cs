namespace Catalogue.Application.Products.Queries.GetProducts
{
    public record GetProductsQuery(int pageSize = 20, int pageNumber = 1)
    {
    }
}
