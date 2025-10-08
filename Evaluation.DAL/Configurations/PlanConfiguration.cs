using Evaluation.DAL.Entities.Planing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder
            .Property(x => x.NameAr)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(x => x.NameEn)
            .IsRequired()
            .HasMaxLength(200);
    }
}
