namespace AudioStreaming.Application.DTOs.Responses
{
    public class PagedList<T> : List<T>
    {
        public PaginationMetadata PaginationMetadata { get; init; } = new();


        public PagedList(List<T> items, int totalCount, int currentPage, int pageSize)
        {
            PaginationMetadata.TotalCount = totalCount;
            PaginationMetadata.PageSize = pageSize;
            PaginationMetadata.CurrentPage = currentPage;
            PaginationMetadata.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            AddRange(items);
        }
    }
}
