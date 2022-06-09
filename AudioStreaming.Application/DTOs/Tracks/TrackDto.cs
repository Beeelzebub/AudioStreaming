namespace AudioStreaming.Application.DTOs.Tracks
{
    public class TrackDto
    {
        public int TrackId { get; set; }

        public string Name { get; set; }

        public int ReleaseId { get; set; }

        public string CoverUri { get; set; }

        public List<string> Genres { get; set; } = new();

        public List<TrackParticipantDto> TrackParticipants { get; set; } = new();
    }
}
