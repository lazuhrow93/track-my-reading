using Database.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration;

public class ReadingStatusConfiguration : EntityTypeConfiguration<ReadingStatus>, IEntityTypeConfiguration<ReadingStatus>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ReadingStatus> builder)
    {
        builder.Property(e => e.Description)
            .HasMaxLength(50)
            .IsRequired(true);

        builder.Property(e => e.Percentage)
            .HasPrecision(5, 2)
            .HasDefaultValue(0m);
    }
}
