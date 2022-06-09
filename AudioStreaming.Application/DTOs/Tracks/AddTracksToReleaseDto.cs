using AudioStreaming.Application.DTOs.Releases;

namespace AudioStreaming.Application.DTOs.Tracks
{
    public class AddTracksToReleaseDto : ReleaseBaseDto
    {
        public ICollection<AddTrackDto> Tracks { get; set; } = new List<AddTrackDto>();
    }
}
