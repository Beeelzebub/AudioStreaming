﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Persistence.Configurations
{
    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(e => e.PlaylistCoverUri)
                .IsRequired(false);

            builder.Property(e => e.IsPrivate)
                .IsRequired(true);

            builder.HasMany(e => e.Permissions)
                .WithOne(e => e.Playlist)
                .HasForeignKey(e => e.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Owner)
                .WithMany(e => e.FavoritePlaylists)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.Songs)
                .WithMany(e => e.Playlists);
        }
    }
}
