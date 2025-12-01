using Database.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration;

public class AuthorConfiguration : EntityTypeConfiguration<Author>, IEntityTypeConfiguration<Author>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Author> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(500)
            .IsRequired(true);
    }
}
