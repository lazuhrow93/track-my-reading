using Database.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration;

public class TraitConfiguration : EntityTypeConfiguration<Trait>, IEntityTypeConfiguration<Trait>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Trait> builder)
    {
        builder.Property(e => e.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasMany(e => e.Characters)
            .WithMany(c => c.Traits);
    }
}
