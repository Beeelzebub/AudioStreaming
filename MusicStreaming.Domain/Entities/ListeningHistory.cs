using MusicStreaming.Domain.Abstractions;

namespace MusicStreaming.Domain.Entities
{
    public class ListeningHistory 
    {
        public DateTimeOffset Date { get; set; }

        public int UserId { get; set; }

        public int SongId { get; set; }

        public User User { get; set; } = default!;

        public Song Song { get; set; } = default!;
    }
}
