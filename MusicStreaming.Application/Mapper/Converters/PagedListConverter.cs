using AutoMapper;
using MusicStreaming.Application.DTOs.Responses;

namespace MusicStreaming.Application.Mapper.Converters
{
    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var items = source.Select(e => context.Mapper.Map<TSource, TDestination>(e)).ToList();

            return new PagedList<TDestination>(items, source.PaginationMetadata.TotalPages, source.PaginationMetadata.CurrentPage, source.PaginationMetadata.PageSize);
        }
    }
}
