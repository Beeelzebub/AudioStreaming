using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Persistence.Configurations
{
    public class ListeningHistoryConfiguration : IEntityTypeConfiguration<ListeningHistory>
    {
        public void Configure(EntityTypeBuilder<ListeningHistory> builder)
        {
            builder.HasKey(e => new { e.UserId, e.SongId, e.Date });

            builder.HasOne(e => e.User)
                .WithMany(e => e.ListeningHistory)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Song)
                .WithMany(e => e.ListeningHistory)
                .HasForeignKey(e => e.SongId);
        }
    }
}
