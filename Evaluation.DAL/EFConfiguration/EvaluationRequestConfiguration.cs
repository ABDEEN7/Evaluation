using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration;

public class EvaluationRequestConfiguration : IEntityTypeConfiguration<EvaluationRequest>
{
    public void Configure(EntityTypeBuilder<EvaluationRequest> builder)
    {        
        builder.HasAlternateKey(o => o.Sequence);

        builder.Property(x => x.Sequence).UseHiLo("EvaluationRequest_Sequence");
    }
}
