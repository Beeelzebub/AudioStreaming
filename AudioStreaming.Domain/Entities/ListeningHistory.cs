namespace AudioStreaming.Domain.Entities
{
    public class ListeningHistory 
    {
        public int Id { get; set; }

        public DateTimeOffset Date { get; set; }

        public string? UserId { get; set; }

        public int TrackId { get; set; }

        public User? User { get; set; } = default!;

        public Track Track { get; set; } = default!;
    }
}
