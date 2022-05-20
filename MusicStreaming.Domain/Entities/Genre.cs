using MusicStreaming.Domain.Abstractions;

namespace MusicStreaming.Domain.Entities
{
    public class Genre : IEntity
    {
        public string Name { get; set; } = default!;

        public string? Description { get; set; } = default!; 

        public List<Song> Songs { get; set; } = default!;
    }
}
