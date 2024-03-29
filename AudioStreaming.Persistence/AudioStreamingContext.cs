﻿using Microsoft.EntityFrameworkCore;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AudioStreaming.Persistence
{
    public class AudioStreamingContext : IdentityDbContext<User>, IAudioStreamingContext
    {

        public AudioStreamingContext(DbContextOptions options) 
            : base(options)
        {

        }

        public DbSet<Artist> Artist { get; set; } = default!;

        public DbSet<Genre> Genre { get; set; } = default!;

        public DbSet<ListeningHistory> ListeningHistory { get; set; } = default!;

        public DbSet<PlaylistPermission> PlaylistPermission { get; set; } = default!;

        public DbSet<Playlist> Playlist { get; set; } = default!;

        public DbSet<Release> Release { get; set; } = default!;

        public DbSet<Track> Track { get; set; } = default!;

        public DbSet<Chart> Chart { get; set; } = default!;

        public DbSet<User> User { get; set; } = default!;

        public DbSet<TrackParticipant> TrackParticipant { get; set; } = default!;

        public DbSet<ReleaseParticipant> ReleaseParticipant { get; set; } = default!;

        public DbSet<ReleaseVerificationHistory> ReleaseVerificationHistory { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AudioStreamingContext).Assembly);
        }
    }
}
