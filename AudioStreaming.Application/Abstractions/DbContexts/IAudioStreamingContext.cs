using Microsoft.EntityFrameworkCore;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Application.Abstractions.DbContexts
{
    public interface IAudioStreamingContext
    {
        DbSet<Artist> Artist { get; }

        DbSet<Genre> Genre { get; }

        DbSet<ListeningHistory> ListeningHistory { get; }

        DbSet<TrackParticipant> TrackParticipant { get; }

        DbSet<ReleaseParticipant> ReleaseParticipant { get; }

        DbSet<PlaylistPermission> PlaylistPermission { get; }

        DbSet<ReleaseVerificationHistory> ReleaseVerificationHistory { get; }

        DbSet<Playlist> Playlist { get; }

        DbSet<Release> Release { get; }

        DbSet<Track> Track { get; }

        DbSet<Chart> Chart { get; }

        DbSet<User> User { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();
    }
}
