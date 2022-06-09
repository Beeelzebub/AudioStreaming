using System;
using System.Linq;
using System.Threading.Tasks;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Domain.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.ChartUpdater
{
    public class ChartUpdater
    {
        private readonly IAudioStreamingContext _context;

        public ChartUpdater(IAudioStreamingContext context)
        {
            _context = context;
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger logger)
        {
            logger.LogInformation($"C# Timer trigger function executed at: {DateTimeOffset.Now}");

            var chart = await _context.Chart.OrderBy(c => c.Position).ToListAsync();

            var tracksToChart = await _context.Track
                .Where(t => t.Release.Stage == ReleaseStage.Published)
                .OrderBy(t => t.ListeningHistory.Where(l => l.Date >= DateTimeOffset.Now.AddMonths(-1)).Count())
                .Take(chart.Count)
                .ToListAsync();

            for (int i = 0; i < chart.Count; i++)
            {
                chart[i].Track = tracksToChart[i];
            }

            await _context.SaveChangesAsync();
        }
    }
}
