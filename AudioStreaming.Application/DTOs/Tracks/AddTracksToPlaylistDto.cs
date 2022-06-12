using AudioStreaming.Application.DTOs.Playlists;

namespace AudioStreaming.Application.DTOs.Tracks
{
    public class AddTracksToPlaylistDto : PlaylistBaseDto
    {
        public ICollection<int> TrackIds { get; set; }
    }
}
