using Evaluation.DAL.Entities.FormsModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        // Primary key
        builder.HasKey(i => i.Id);

        // Required and max length constraints for strings
        builder.Property(i => i.NameAr)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.NameEn)
            .IsRequired()
            .HasMaxLength(200);

        // Decimal precision
        builder.Property(i => i.Min)
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.Max)
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.Weight)
            .IsRequired();

        builder.Property(i => i.IsEvaluation)
            .IsRequired();

        builder.Property(i => i.FormId).IsRequired();
        builder.Property(i => i.ScopeId).IsRequired();

        builder.HasOne(i => i.CalcMethod)
            .WithMany()
            .HasForeignKey(i => i.CalcMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
