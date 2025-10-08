using Evaluation.DAL.Entities.Planing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Evaluation.DAL.Configurations;

public class ChangeRequestConfiguration : IEntityTypeConfiguration<ChangeRequest>
{
    public void Configure(EntityTypeBuilder<ChangeRequest> builder)
    {
        builder.Property(cr => cr.NameAr)
                  .IsRequired()
                  .HasMaxLength(200);

        builder.Property(cr => cr.NameEn)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cr => cr.Notes)
            .HasMaxLength(1000);

        builder.HasOne(cr => cr.Plan)
            .WithMany()
            .HasForeignKey(cr => cr.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cr => cr.ChangeRequestType)
            .WithMany()
            .HasForeignKey(cr => cr.RequestTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cr => cr.User)
            .WithMany()
            .HasForeignKey(cr => cr.RequestedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
