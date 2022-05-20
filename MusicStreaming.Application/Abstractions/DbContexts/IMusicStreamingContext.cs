using Microsoft.EntityFrameworkCore;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Application.Abstractions.DbContexts
{
    public interface IMusicStreamingContext
    {
        DbSet<Artist> Artist { get; }

        DbSet<Genre> Genre { get; }

        DbSet<ListeningHistory> ListeningHistory { get; }

        DbSet<SongParticipant> SongParticipant { get; }

        DbSet<ReleaseParticipant> ReleaseParticipant { get; }

        DbSet<Permission> Permission { get; }

        DbSet<Playlist> Playlist { get; }

        DbSet<Release> Release { get; }

        DbSet<Song> Song { get; }

        DbSet<User> User { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
