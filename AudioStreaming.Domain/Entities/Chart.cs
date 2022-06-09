namespace AudioStreaming.Domain.Entities
{
    public class Chart
    {
        public int Position { get; set; }

        public int TrackId { get; set; }

        public Track Track { get; set; }
    }
}
