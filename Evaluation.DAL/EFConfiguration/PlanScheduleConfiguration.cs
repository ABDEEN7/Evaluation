using Evaluation.DAL.Entities.Planing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration;

internal class PlanScheduleConfiguration : IEntityTypeConfiguration<PlanSchedule>
{
    public void Configure(EntityTypeBuilder<PlanSchedule> builder)
    {
        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId);

        builder.HasOne(x => x.School)
            .WithMany()
            .HasForeignKey(x => x.SchoolId);

    }
}
