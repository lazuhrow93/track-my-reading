using Database.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration;

public class BookConfiguration : EntityTypeConfiguration<Book>, IEntityTypeConfiguration<Book>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Book> builder)
    {
        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .IsRequired(true);

        builder.HasOne(e => e.Author)
            .WithMany()
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ReadingStatus)
            .WithMany()
            .HasForeignKey(e => e.ReadingStatusId);
    }
}
