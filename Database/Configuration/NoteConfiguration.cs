using Database.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configuration;

public class NoteConfiguration : EntityTypeConfiguration<Note>, IEntityTypeConfiguration<Note>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Note> builder)
    {
        builder.Property(e => e.Value)
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasOne(e => e.Character)
            .WithMany(c => c.Notes)
            .HasForeignKey(e => e.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
