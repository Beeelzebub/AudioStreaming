using MusicStreaming.Domain.Abstractions;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Domain.Entities
{
    public class Permission : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public PermissionType Type { get; set; }

        public int PlaylistId { get; set; }

        public Playlist Playlist { get; set; } = default!;

        public List<User> Users { get; set; } = default!;
    }
}
