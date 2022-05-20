using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Response;
using MusicStreaming.Application.Abstractions.Services.BlobStorage;
using MusicStreaming.Application.DTOs.Response;
using MusicStreaming.Application.DTOs.Songs;
using MusicStreaming.Application.Mediator.Common.Commands;
using MusicStreaming.Domain.Entities;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.Mediator.Songs.Commands
{
    public record AddSongsToReleaseCommand(IEnumerable<AddSongDto> Songs, int ReleaseId) : ICommand<IEnumerable<int>>;

    public class AddSongsToReleaseHandler : ICommandHandler<AddSongsToReleaseCommand, IEnumerable<int>>
    {
        private readonly IMusicStreamingContext _context;
        private readonly ISongBlobService _songBlobService;
        private readonly ILogger _logger;

        public AddSongsToReleaseHandler(IMusicStreamingContext context, ILogger logger, ISongBlobService songBlobService)
        {
            _context = context;
            _logger = logger;
            _songBlobService = songBlobService;
        }

        public async Task<IApiResult<IEnumerable<int>>> Handle(AddSongsToReleaseCommand request, CancellationToken cancellationToken)
        {
            IApiResult<IEnumerable<int>>? errorResult = null;
            
            var release = await _context.Release
                .Include(r => r.Songs)
                .SingleOrDefaultAsync(r => r.Id == request.ReleaseId);

            if (!CheckRelease(release, request, out errorResult))
            {
                return errorResult;
            }

            var artistsIds = request.Songs
                .SelectMany(s => s.Participants)
                .Select(s => s.ArtistId);
            var artists = await _context.Artist
                .Where(a => artistsIds.Contains(a.Id))
                .ToListAsync();

            if (!CheckArtists(artists, artistsIds, out errorResult))
            {
                return errorResult;
            }

            var genresIds = request.Songs.SelectMany(g => g.Genres);
            var genres = await _context.Genre
                .Where(g => genresIds.Contains(g.Name))
                .ToListAsync();

            if (!CheckGenres(genres, genresIds, out errorResult))
            {
                return errorResult;
            }

            var songsToAdd = await CreateSongsListToAdd(release, request.Songs, genres, artists);

            release.Songs.AddRange(songsToAdd);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResult<IEnumerable<int>>.CreateSuccessfulResult(songsToAdd.Select(s => s.Id));
        }

        private bool CheckRelease(Release release, AddSongsToReleaseCommand request, out IApiResult<IEnumerable<int>>? errorResult)
        {
            errorResult = null;

            if (release == null)
            {
                _logger.LogError("Release with id {id} not found.", request.ReleaseId);
                errorResult = ApiResult<IEnumerable<int>>.CreateFailedResult($"Release with id {request.ReleaseId} not found.");
            }

            if (release.Stage != ReleaseStage.Editing)
            {
                errorResult = ApiResult<IEnumerable<int>>.CreateFailedResult("The release is not at the editing stage. You cannot add tracks.");
            }

            if (release.Type == ReleaseType.Single && release.Songs.Count != 0)
            {
                errorResult = ApiResult<IEnumerable<int>>.CreateFailedResult("The specified release type is single and you have already added a track earlier. " +
                    "Change the release type or delete a previously added track.");
            }

            if (release.Type == ReleaseType.Single && request.Songs.Count() > 1)
            {
                errorResult = ApiResult<IEnumerable<int>>.CreateFailedResult("the specified release type is single but you are trying to add several tracks. " +
                    "Change the release type or submit one track.");
            }

            return errorResult == null;
        }

        private bool CheckArtists(IEnumerable<Artist> artists, IEnumerable<int> requestedIds, out IApiResult<IEnumerable<int>>? errorResult)
        {
            errorResult = null;

            var existingIds = artists.Select(s => s.Id);

            if (existingIds.Count() != requestedIds.Count())
            {
                var nonexistentIds = requestedIds.Except(existingIds);
                var ending = nonexistentIds.Count() > 1 ? "s" : "";
                var errorMessage = $"Artist{ending} with id{ending} {String.Join(',', nonexistentIds)} not exist{ending}.";
                
                _logger.LogError(errorMessage);

                errorResult = ApiResult<IEnumerable<int>>.CreateFailedResult(errorMessage);
            }

            return errorResult == null;
        }

        private bool CheckGenres(IEnumerable<Genre> genres, IEnumerable<string> requestedIds, out IApiResult<IEnumerable<int>>? errorResult)
        {
            errorResult = null;

            var existingIds = genres.Select(e => e.Name);

            if (existingIds.Count() != requestedIds.Count())
            {
                var nonexistentGenres = requestedIds.Except(existingIds);
                var ending = nonexistentGenres.Count() > 1 ? "s" : "";
                var errorMessage = $"Genre{ending} with name{ending} {String.Join(',', nonexistentGenres)} not exist{ending}.";
                _logger.LogError(errorMessage);

                errorResult = ApiResult<IEnumerable<int>>.CreateFailedResult(errorMessage);
            }

            return errorResult == null;
        }

        private async Task<IEnumerable<Song>> CreateSongsListToAdd(Release release, IEnumerable<AddSongDto> songs, IEnumerable<Genre> genres, IEnumerable<Artist> artists)
        {
            var songsToAdd = new List<Song>();

            foreach (var song in songs)
            {
                var path = await UploadSongToStorage(song, release.Title);
                var songParticipants = song.Participants.Select(p => new SongParticipant { ArtistId = p.ArtistId, Order = p.Order, Role = p.Role }).ToList();
                var songGenres = genres.Where(e => song.Genres.Contains(e.Name)).ToList();

                var songToAdd = new Song
                {
                    Name = song.Name,
                    Release = release,
                    PathInStorage = path,
                    Genres = songGenres,
                    Participants = songParticipants
                };

                songsToAdd.Add(songToAdd);
            }

            return songsToAdd;
        }

        private async Task<string> UploadSongToStorage(AddSongDto song, string releaseName)
        {
            await using var stream = new MemoryStream();
            var fileName = $"{song.Name}.mp3";
            await song.File.CopyToAsync(stream);

            return await _songBlobService.UploadAsync(releaseName, fileName, stream);
        }
    }
}
