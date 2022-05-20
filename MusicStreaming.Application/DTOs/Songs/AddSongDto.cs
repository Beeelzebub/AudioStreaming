
using Microsoft.AspNetCore.Http;
using MusicStreaming.Application.DTOs.Artists;

namespace MusicStreaming.Application.DTOs.Songs
{
    public class AddSongDto
    {
        public string Name { get; set; }

        public IFormFile File { get; set; }

        public IEnumerable<SongParticipantDto> Participants { get; set; } 

        public IEnumerable<string> Genres { get; set; } 
    }
}
