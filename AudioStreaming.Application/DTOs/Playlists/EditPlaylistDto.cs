using Microsoft.AspNetCore.Http;

namespace AudioStreaming.Application.DTOs.Playlists
{
    public class EditPlaylistDto : PlaylistBaseDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public IFormFile PlaylistCoverFile { get; set; }
    }
}
