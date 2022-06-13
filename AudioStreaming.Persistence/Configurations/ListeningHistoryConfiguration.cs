using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Persistence.Configurations
{
    public class ListeningHistoryConfiguration : IEntityTypeConfiguration<ListeningHistory>
    {
        public void Configure(EntityTypeBuilder<ListeningHistory> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Date)
                .IsRequired(true);

            builder.HasOne(e => e.User)
                .WithMany(e => e.ListeningHistory)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Track)
                .WithMany(e => e.ListeningHistory)
                .HasForeignKey(e => e.TrackId);
        }
    }
}
