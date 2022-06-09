using Microsoft.AspNetCore.Http;

namespace AudioStreaming.Application.DTOs.Tracks
{
    public class AddTrackDto
    {
        public string Name { get; set; }

        public IFormFile File { get; set; }

        public ICollection<TrackParticipantToAddDto> Participants { get; set; } 

        public ICollection<string> Genres { get; set; } 
    }
}
