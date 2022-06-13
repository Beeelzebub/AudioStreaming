using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Releases;
using AudioStreaming.Application.DTOs.Requests;
using AudioStreaming.Application.Mediator.Common.Queries;
using AudioStreaming.Application.Extensions;
using AudioStreaming.Domain.Enums;
using AudioStreaming.Application.DTOs.Responses;

namespace AudioStreaming.Application.Mediator.Releases.Queries
{
    public record GetReleaseListQuery(RequestParameters Parameters, ICollection<ReleaseStage>? ReleaseStages = null, 
        string? ArtistId = null) : IQuery<PagedList<ReleaseDto>>;

    public class GetReleaseListHandler : IQueryHandler<GetReleaseListQuery, PagedList<ReleaseDto>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IMapper _mapper;

        public GetReleaseListHandler(IAudioStreamingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IApiResult<PagedList<ReleaseDto>>> Handle(GetReleaseListQuery request, CancellationToken cancellationToken)
        {
            var releasesQuery = _context.Release
                .AsNoTracking()
                .OrderBy(r => r.Date)
                .Include(r => r.Participants)
                .Include(r => r.Artists)
                .AsQueryable();

            if (request.ReleaseStages != null && request.ReleaseStages.Count != 0)
            {
                releasesQuery = releasesQuery.Where(r => request.ReleaseStages.Contains(r.Stage));
            }

            if (!string.IsNullOrEmpty(request.ArtistId))
            {
                releasesQuery = releasesQuery.Where(r => r.Participants.Select(p => p.ArtistId).Contains(request.ArtistId));
            }

            var searchString = request.Parameters.SearchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                releasesQuery = releasesQuery.Where(r => r.Title.Contains(searchString) || r.Artists.Select(a => a.Pseudonym).Any(a => a.Contains(searchString)));
            }

            var result = await releasesQuery.ToPagedListAsync(request.Parameters.Page, request.Parameters.PageSize, cancellationToken);

            return ApiResult<PagedList<ReleaseDto>>.CreateSuccessfulResult(_mapper.Map<PagedList<ReleaseDto>>(result));
        }
    }
}
