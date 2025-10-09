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
    }
}
