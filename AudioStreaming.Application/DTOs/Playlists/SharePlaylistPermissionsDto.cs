using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.DTOs.Playlists
{
    public class SharePlaylistPermissionsDto
    {
        public string UserId { get; set; }

        public PermissionType PermissionType { get; set; }
    }
}
