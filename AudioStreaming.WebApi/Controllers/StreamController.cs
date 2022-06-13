using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Common.Extensions;
using AudioStreaming.Domain.Entities;
using AudioStreaming.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AudioStreaming.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        private readonly ITrackBlobService _trackBlobService;
        private readonly IAudioStreamingContext _dbContext;

        public StreamController(ITrackBlobService trackBlobService, IAudioStreamingContext dbContext)
        {
            _trackBlobService = trackBlobService;
            _dbContext = dbContext;
        }

        [HttpGet("[action]/{trackId}")]
        public async Task<IActionResult> GetTrackStream([FromRoute] int trackId, CancellationToken cancellationToken)
        {
            var track = await _dbContext.Track
                .Include(t => t.Release)
                    .ThenInclude(r => r.Artists)
                .SingleOrDefaultAsync(t => t.Id == trackId, cancellationToken);

            if (track == null)
            {
                return BadRequest($"Track with id {trackId} not found.");
            }

            if (track.Release.Stage != ReleaseStage.Published)
            {
                if (!User.Identity?.IsAuthenticated ?? false)
                {
                    return Unauthorized();
                }
                if (!User.IsInRole("Moderator"))
                {
                    if (User.IsInRole("Artist"))
                    {
                        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        if (!track.Release.Artists.Any(a => a.Id == userId))
                        {
                            return Forbid();
                        }
                    }
                    else
                    {
                        return Forbid();
                    }
                }
            }

            var stream = await _trackBlobService.GetStreamAsync(track.PathInStorage);

            if (stream == null)
            {
                return StatusCode(500);
            }

            track.ListeningHistory.Add (new ListeningHistory { Date = DateTimeOffset.UtcNow, UserId = User.Identity?.IsAuthenticated ?? false ? User.GetUserId() : null });

            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = File(stream, "audio/mpeg");
            response.EnableRangeProcessing = true;

            return response;
        }
    }
}
