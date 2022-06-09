using AudioStreaming.Domain.Abstractions;

namespace AudioStreaming.Domain.Entities
{
    public class ReleaseParticipant : IEntity
    {
        public string ArtistId { get; set; }

        public int ReleaseId { get; set; }

        public byte Order { get; set; }

        public Artist Artist { get; set; } = default!;

        public Release Release { get; set; } = default!;
    }
}
