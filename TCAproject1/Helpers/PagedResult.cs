namespace TCAproject1.Helpers
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<T> Data { get; set; } = new();
    }

}
