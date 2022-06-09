namespace AudioStreaming.Domain.Entities
{
    public class ListeningHistory 
    {
        public DateTimeOffset Date { get; set; }

        public int UserId { get; set; }

        public int TrackId { get; set; }

        public User User { get; set; } = default!;

        public Track Track { get; set; } = default!;
    }
}
