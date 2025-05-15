using Microsoft.EntityFrameworkCore;

namespace TCAproject1.Helpers
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            int page,
            int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 10;

            var total = await query.CountAsync();
            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                TotalItems = total,
                Page = page,
                PageSize = pageSize,
                Data = data
            };
        }
    }

}
