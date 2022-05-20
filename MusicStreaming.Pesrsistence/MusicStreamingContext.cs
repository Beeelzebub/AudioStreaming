using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Persistence
{
    public class MusicStreamingContext : DbContext, IMusicStreamingContext
    {

        public MusicStreamingContext(DbContextOptions options) 
            : base(options)
        {

        }

        public DbSet<Artist> Artist { get; set; } = default!;

        public DbSet<Genre> Genre { get; set; } = default!;

        public DbSet<ListeningHistory> ListeningHistory { get; set; } = default!;

        public DbSet<Permission> Permission { get; set; } = default!;

        public DbSet<Playlist> Playlist { get; set; } = default!;

        public DbSet<Release> Release { get; set; } = default!;

        public DbSet<Song> Song { get; set; } = default!;

        public DbSet<User> User { get; set; } = default!;

        public DbSet<SongParticipant> SongParticipant { get; set; } = default!;

        public DbSet<ReleaseParticipant> ReleaseParticipant { get; set; } = default!;


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicStreamingContext).Assembly);
        }
    }
}
