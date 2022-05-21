using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.DTOs.Responses;

namespace MusicStreaming.Application.Extensions
{
    public static class QueryableExtensions
    {
		public static async Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
		{
			var count = await source.CountAsync();
			var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

			return new PagedList<T>(items, count, pageNumber, pageSize);
		}
	}
}
