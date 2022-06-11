using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Services.BlobStorage;
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

        public StreamController(ITrackBlobService trackBlobService)
        {
            _trackBlobService = trackBlobService;
        }

        public async Task<IActionResult> GetTrackStream(int trackId, CancellationToken cancellationToken)
        {
            var track = await _dbContext.Track
                .AsNoTracking()
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

            var response = File(stream, "audio/mpeg");
            response.EnableRangeProcessing = true;

            return response;
        }
    }
}
