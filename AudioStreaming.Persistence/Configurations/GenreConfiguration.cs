using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Persistence.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(e => e.Name);

            builder.Property(e => e.Name)
                .HasMaxLength(50);

            builder.Property(e => e.Description)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.HasMany(e => e.Tracks)
                .WithMany(e => e.Genres);
        }
    }
}
