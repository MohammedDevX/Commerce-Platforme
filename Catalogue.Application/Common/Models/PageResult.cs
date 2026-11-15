namespace Catalogue.Application.Common.Models
{
    public record PageResult<T>(IReadOnlyList<T> items, 
        int totalCount, // Total products in DB 
        int pageNumber, 
        int pageSize)
    {
        public int TotalItems => items.Count; // Total products returned
        public int TotalPages => (int)Math.Ceiling(totalCount / (double)pageSize); // Total pages to handler in front end
        public bool HasPrevious => pageNumber > 1;
        public bool HasNext => TotalPages > pageNumber;
    }
}
