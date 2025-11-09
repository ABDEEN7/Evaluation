using Evaluation.DAL.Entities.Planing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration;

internal class PlanScheduleConfiguration : IEntityTypeConfiguration<EvaluationRequest>
{
    public void Configure(EntityTypeBuilder<EvaluationRequest> builder)
    {
        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId);

        builder.HasOne(x => x.School)
            .WithMany()
            .HasForeignKey(x => x.SchoolId);
    }
}
