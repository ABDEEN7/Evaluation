using Evaluation.DAL.Entities.Planing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration;

public class ChangeRequestConfiguration : IEntityTypeConfiguration<ChangeRequest>
{
    public void Configure(EntityTypeBuilder<ChangeRequest> builder)
    {
        builder.HasOne(c => c.Plan)
            .WithMany()
            .HasForeignKey(c => c.PlanId);
    }
}
