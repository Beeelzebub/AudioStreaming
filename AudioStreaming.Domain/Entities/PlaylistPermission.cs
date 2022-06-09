using AudioStreaming.Domain.Abstractions;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Domain.Entities
{
    public class PlaylistPermission : IEntity
    {
        public int PlaylistId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = default!;

        public PermissionType Type { get; set; }

        public Playlist Playlist { get; set; } = default!;

        public User User { get; set; } = default!;
    }
}
