using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Genres;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Queries;

namespace AudioStreaming.Application.Mediator.Genres.Queries
{
    public record GetGenreListQuery : IQuery<ICollection<GenreDto>>;

    public class GetGenreListQueryHandler : IQueryHandler<GetGenreListQuery, ICollection<GenreDto>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IMapper _mapper;

        public GetGenreListQueryHandler(IAudioStreamingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IApiResult<ICollection<GenreDto>>> Handle(GetGenreListQuery request, CancellationToken cancellationToken)
        {
            var genres = await _context.Genre.ToListAsync(cancellationToken);
            
            return ApiResult<ICollection<GenreDto>>.CreateSuccessfulResult(_mapper.Map<ICollection<GenreDto>>(genres));
        }
    }
}
