using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Responses;
using MusicStreaming.Application.DTOs.Releases;
using MusicStreaming.Application.DTOs.Requests;
using MusicStreaming.Application.Mediator.Common.Queries;
using MusicStreaming.Application.Extensions;
using MusicStreaming.Domain.Enums;
using MusicStreaming.Application.DTOs.Responses;

namespace MusicStreaming.Application.Mediator.Releases.Queries
{
    public record GetReleaseListQuery(RequestParameters Parameters, int ArtistId = 0) : IQuery<PagedList<ReleaseDto>>;

    public class GetReleaseListHandler : IQueryHandler<GetReleaseListQuery, PagedList<ReleaseDto>>
    {
        private readonly IMusicStreamingContext _context;
        private readonly IMapper _mapper;

        public GetReleaseListHandler(IMusicStreamingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IApiResult<PagedList<ReleaseDto>>> Handle(GetReleaseListQuery request, CancellationToken cancellationToken)
        {
            var releases = _context.Release.AsNoTracking()
                .OrderBy(r => r.Date)
                .Include(r => r.Participants)
                .Where(r => r.Stage == ReleaseStage.Released);

            if (request.ArtistId != 0)
            {
                releases = releases.Where(r => r.Participants.Select(p => p.ArtistId).Contains(request.ArtistId));
            }

            var searchString = request.Parameters.SearchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                releases = releases.Where(r => r.Title.Contains(searchString) || r.Artists.Select(a => a.Pseudonym).Any(a => a.Contains(searchString)));
            }

            var result = await releases.ToPagedList(request.Parameters.Page, request.Parameters.PageSize, cancellationToken);

            return ApiResult<PagedList<ReleaseDto>>.CreateSuccessfulResult(_mapper.Map<PagedList<ReleaseDto>>(result));
        }
    }
}
