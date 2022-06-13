using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Infrastructure.BackgroundJobs
{
    public class ChartUpdater
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger<ChartUpdater> _logger;

        public ChartUpdater(IAudioStreamingContext context, ILogger<ChartUpdater> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Update()
        {
            _logger.LogInformation($"Chart updater executed at: {DateTimeOffset.Now}");

            var chart = await _context.Chart.OrderBy(c => c.Position).ToListAsync();

            var tracksToChart = await _context.Track
                .Where(t => t.Release.Stage == ReleaseStage.Published)
                .OrderByDescending(t => t.ListeningHistory.Where(l => l.Date >= DateTimeOffset.Now.AddMonths(-1)).Count())
                .Take(chart.Count)
                .ToListAsync();

            foreach (var position in chart)
            {
                var previousTrack = position.Track;

                if (previousTrack != null)
                    previousTrack.PositionInChart = null;
            }

            await _context.SaveChangesAsync();

            for (int i = 0; (tracksToChart.Count == chart.Count && i < chart.Count) || i < tracksToChart.Count; i++)
            {
                chart[i].Track = tracksToChart[i];
            }

            await _context.SaveChangesAsync();
        }
    }
}
