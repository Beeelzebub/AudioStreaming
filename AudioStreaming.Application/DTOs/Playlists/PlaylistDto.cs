namespace AudioStreaming.Application.DTOs.Playlists
{
    public class PlaylistDto : PlaylistBaseDto
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public string PlaylistCoverUri { get; set; }
    }
}
