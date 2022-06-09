using AudioStreaming.Domain.Abstractions;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Domain.Entities
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
