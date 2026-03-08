using Database.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration;

public class CharacterConfiguration : EntityTypeConfiguration<Character>, IEntityTypeConfiguration<Character>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Character> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(500)
            .IsRequired(true);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.HasOne(e => e.Book)
            .WithMany()
            .HasForeignKey(e => e.BookId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
