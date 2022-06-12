using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.DTOs.Tracks;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Entities;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Tracks.Commands
{
    public record AddTracksToReleaseCommand(ICollection<AddTrackDto> Tracks, int ReleaseId) : ICommand<ICollection<int>>;

    public class AddTracksToReleaseHandler : ICommandHandler<AddTracksToReleaseCommand, ICollection<int>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ITrackBlobService _TrackBlobService;
        private readonly ILogger<AddTracksToReleaseHandler> _logger;

        public AddTracksToReleaseHandler(IAudioStreamingContext context, ILogger<AddTracksToReleaseHandler> logger, ITrackBlobService TrackBlobService)
        {
            _context = context;
            _logger = logger;
            _TrackBlobService = TrackBlobService;
        }

        public async Task<IApiResult<ICollection<int>>> Handle(AddTracksToReleaseCommand request, CancellationToken cancellationToken)
        {
            IApiResult<ICollection<int>>? errorResult = null;
            
            var release = await _context.Release
                .Include(r => r.Tracks)
                .SingleOrDefaultAsync(r => r.Id == request.ReleaseId);

            if (!CheckRelease(release, request, out errorResult))
            {
                return errorResult;
            }

            var artistsIds = request.Tracks
                .SelectMany(s => s.Participants)
                .Select(s => s.ArtistId)
                .ToList();
            var artists = await _context.Artist
                .Where(a => artistsIds.Contains(a.Id))
                .ToListAsync();

            if (!CheckArtists(artists, artistsIds, out errorResult))
            {
                return errorResult;
            }

            var genresIds = request.Tracks.SelectMany(g => g.Genres);
            var genres = await _context.Genre
                .Where(g => genresIds.Contains(g.Name))
                .ToListAsync();

            if (!CheckGenres(genres, genresIds, out errorResult))
            {
                return errorResult;
            }

            var TracksToAdd = await CreateTracksListToAdd(release, request.Tracks, genres, artists);

            release.Tracks.AddRange(TracksToAdd);
            await _context.SaveChangesAsync(cancellationToken);



            return ApiResult<ICollection<int>>.CreateSuccessfulResult(TracksToAdd.Select(s => s.Id).ToList());
        }

        private bool CheckRelease(Release release, AddTracksToReleaseCommand request, out IApiResult<ICollection<int>>? errorResult)
        {
            errorResult = null;

            if (release == null)
            {
                _logger.LogError("Release with id {id} not found.", request.ReleaseId);
                errorResult = ApiResult<ICollection<int>>.CreateFailedResult($"Release with id {request.ReleaseId} not found.");
            }

            if (release.Stage != ReleaseStage.Editing)
            {
                errorResult = ApiResult<ICollection<int>>.CreateFailedResult("The release is not at the editing stage. You cannot add tracks.");
            }

            if (release.Type == ReleaseType.Single && release.Tracks.Count != 0)
            {
                errorResult = ApiResult<ICollection<int>>.CreateFailedResult("The specified release type is single and you have already added a track earlier. " +
                    "Change the release type or delete a previously added track.");
            }

            if (release.Type == ReleaseType.Single && request.Tracks.Count() > 1)
            {
                errorResult = ApiResult<ICollection<int>>.CreateFailedResult("the specified release type is single but you are trying to add several tracks. " +
                    "Change the release type or submit one track.");
            }

            return errorResult == null;
        }

        private bool CheckArtists(IEnumerable<Artist> artists, IEnumerable<string> requestedIds, out IApiResult<ICollection<int>>? errorResult)
        {
            errorResult = null;

            var existingIds = artists.Select(s => s.Id);

            if (existingIds.Count() != requestedIds.Count())
            {
                var nonexistentIds = requestedIds.Except(existingIds);
                var ending = nonexistentIds.Count() > 1 ? "s" : "";
                var errorMessage = $"Artist{ending} with id{ending} {String.Join(',', nonexistentIds)} not exist{ending}.";
                
                _logger.LogError(errorMessage);

                errorResult = ApiResult<ICollection<int>>.CreateFailedResult(errorMessage);
            }

            return errorResult == null;
        }

        private bool CheckGenres(IEnumerable<Genre> genres, IEnumerable<string> requestedIds, out IApiResult<ICollection<int>>? errorResult)
        {
            errorResult = null;

            var existingIds = genres.Select(e => e.Name);

            if (existingIds.Count() != requestedIds.Count())
            {
                var nonexistentGenres = requestedIds.Except(existingIds);
                var ending = nonexistentGenres.Count() > 1 ? "s" : "";
                var errorMessage = $"Genre{ending} with name{ending} {String.Join(',', nonexistentGenres)} not exist{ending}.";
                _logger.LogError(errorMessage);

                errorResult = ApiResult<ICollection<int>>.CreateFailedResult(errorMessage);
            }

            return errorResult == null;
        }

        private async Task<IEnumerable<Track>> CreateTracksListToAdd(Release release, IEnumerable<AddTrackDto> Tracks, IEnumerable<Genre> genres, IEnumerable<Artist> artists)
        {
            var TracksToAdd = new List<Track>();

            foreach (var track in Tracks)
            {
                var path = await UploadTrackToStorage(track, release.Id);
                var TrackParticipants = track.Participants.Select(p => new TrackParticipant { ArtistId = p.ArtistId, Order = p.Order, Role = p.Role }).ToList();
                var TrackGenres = genres.Where(e => track.Genres.Contains(e.Name)).ToList();

                var TrackToAdd = new Track
                {
                    Name = track.Name,
                    Release = release,
                    PathInStorage = path,
                    Genres = TrackGenres,
                    Participants = TrackParticipants
                };

                TracksToAdd.Add(TrackToAdd);
            }

            return TracksToAdd;
        }

        private async Task<string> UploadTrackToStorage(AddTrackDto Track, int releaseId)
        {
            await using var stream = new MemoryStream();
            var fileName = $"{Track.Name}.mp3";
            await Track.File.CopyToAsync(stream);

            return await _TrackBlobService.UploadAsync(releaseId, fileName, stream);
        }
    }
}
