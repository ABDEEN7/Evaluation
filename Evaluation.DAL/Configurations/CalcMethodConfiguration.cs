using Evaluation.DAL.Entities.FormsModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class CalcMethodConfiguration : IEntityTypeConfiguration<CalcMethod>
{
    public void Configure(EntityTypeBuilder<CalcMethod> builder)
    {
        builder.Property(x => x.NameAr)
                    .IsRequired()
                    .HasMaxLength(200);

        builder.Property(x => x.NameEn)
                    .IsRequired()
                    .HasMaxLength(200);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_CalcMethod_MinPercentage_NotNegative",
                $"{nameof(CalcMethod.MinPercentage)} >= 0"
            );

            t.HasCheckConstraint(
               "CK_CalcMethod_MaxPercentage_NotNegative",
               $"{nameof(CalcMethod.MaxPercentage)} >= 0"
           );

            t.HasCheckConstraint(
               "CK_CalcMethod_MinWeight_NotNegative",
               $"{nameof(CalcMethod.MinWeight)} >= 0"
           );

            t.HasCheckConstraint(
               "CK_CalcMethod_MaxWeight_NotNegative",
               $"{nameof(CalcMethod.MaxWeight)} >= 0"
           );

            t.HasCheckConstraint(
                "CK_CalcMethod_PercentageRange",
                $"{nameof(CalcMethod.MinPercentage)} <= {nameof(CalcMethod.MaxPercentage)}"
            );

            t.HasCheckConstraint(
                "CK_CalcMethod_WeightRange",
                $"{nameof(CalcMethod.MinWeight)} <= {nameof(CalcMethod.MaxWeight)}"
            );

        });

    }
}
