using AudioStreaming.Domain.Abstractions;

namespace AudioStreaming.Domain.Entities
{
    public class Genre : IEntity
    {
        public string Name { get; set; } = default!;

        public string? Description { get; set; } = default!; 

        public List<Track> Tracks { get; set; } = default!;
    }
}
