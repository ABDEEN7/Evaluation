using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration;

public class EvaluationRequestConfiguration : IEntityTypeConfiguration<EvaluationRequest>
{
    public void Configure(EntityTypeBuilder<EvaluationRequest> builder)
    {
        builder
           .HasOne(x => x.Plan)
           .WithMany(p=>p.EvaluationRequests)
           .HasForeignKey(x => x.PlanId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Service)
            .WithMany()
            .HasForeignKey(s => s.ServiceId);
    }
}
