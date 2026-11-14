namespace Catalogue.Application.Common.Models
{
    public record PageResult<T>(IReadOnlyList<T> items, 
        int totalCount,
        int pageNumber, 
        int pageSize)
    {
        public int TotalPages => (int)Math.Ceiling(totalCount / (double)pageSize);
        public bool HasPrevious => pageNumber > 1;
        public bool HasNext => TotalPages > pageNumber;
    }
}
