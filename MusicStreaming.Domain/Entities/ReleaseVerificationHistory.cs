using MusicStreaming.Domain.Abstractions;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Domain.Entities
{
    public class ReleaseVerificationHistory : IEntity
    {
        public int Id { get; set; }

        public int ReleaseId { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Comment { get; set; } = default!;

        public ReleaseStage NewStage { get; set; }

        public Release Release { get; set; }
    }
}
