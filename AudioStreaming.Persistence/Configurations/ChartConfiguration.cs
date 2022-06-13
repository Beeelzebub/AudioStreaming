using AudioStreaming.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioStreaming.Persistence.Configurations
{
    public class ChartConfiguration : IEntityTypeConfiguration<Chart>
    {
        public void Configure(EntityTypeBuilder<Chart> builder)
        {
            builder.HasKey(e => e.Position);

            builder.HasOne(e => e.Track)
                .WithOne()
                .IsRequired(false)
                .HasForeignKey("Track", "PositionInChart");
        }
    }
}
