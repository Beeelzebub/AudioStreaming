using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Responses;
using MusicStreaming.Application.DTOs.Genres;
using MusicStreaming.Application.DTOs.Responses;
using MusicStreaming.Application.Mediator.Common.Queries;

namespace MusicStreaming.Application.Mediator.Genres.Queries
{
    public record GetGenreListQuery : IQuery<IEnumerable<GenreDto>>;

    public class GetGenreListQueryHandler : IQueryHandler<GetGenreListQuery, IEnumerable<GenreDto>>
    {
        private readonly IMusicStreamingContext _context;
        private readonly IMapper _mapper;

        public GetGenreListQueryHandler(IMusicStreamingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IApiResult<IEnumerable<GenreDto>>> Handle(GetGenreListQuery request, CancellationToken cancellationToken)
        {
            var genres = await _context.Genre.Select(e => e.Name).ToListAsync(cancellationToken);
            
            return ApiResult<IEnumerable<GenreDto>>.CreateSuccessfulResult(_mapper.Map<List<GenreDto>>(genres));
        }
    }
}
