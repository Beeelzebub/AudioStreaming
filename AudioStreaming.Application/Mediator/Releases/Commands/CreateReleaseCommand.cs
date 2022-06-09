using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Releases;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Entities;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Releases.Commands
{
    public record CreateReleaseCommand(CreateReleaseDto Release) : ICommand<int>;

    public class CreateReleaseHandler : ICommandHandler<CreateReleaseCommand, int>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CreateReleaseHandler(IAudioStreamingContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IApiResult<int>> Handle(CreateReleaseCommand request, CancellationToken cancellationToken)
        {
            var releaseDto = request.Release;

            var artists = await _context.Artist
                .Where(e => releaseDto.ParticipantsIds.Contains(e.Id))
                .ToListAsync();

            if (!CheckArtists(artists, releaseDto.ParticipantsIds, out IApiResult<int>? errorResult))
            {
                return errorResult;
            }

            var releaseToAdd = _mapper.Map<Release>(releaseDto);
            releaseToAdd.Stage = ReleaseStage.Editing;
            releaseToAdd.Artists = artists;

            _context.Release.Add(releaseToAdd);
            await _context.SaveChangesAsync();

            return ApiResult<int>.CreateSuccessfulResult(releaseToAdd.Id);
        }

        private bool CheckArtists(IEnumerable<Artist> artists, IEnumerable<int> requestedIds, out IApiResult<int>? errorResult)
        {
            errorResult = null;

            if (artists.Count() != requestedIds.Count())
            {
                var nonexistentIds = requestedIds.Except(artists.Select(e => e.Id));
                var ending = nonexistentIds.Count() > 1 ? "s" : "";
                var errorMessage = $"Artist{ending} with id{ending} {String.Join(',', nonexistentIds)} not found.";

                _logger.LogError(errorMessage);
                errorResult = ApiResult<int>.CreateFailedResult(errorMessage);
            }

            return errorResult == null;
        }
    }
}
