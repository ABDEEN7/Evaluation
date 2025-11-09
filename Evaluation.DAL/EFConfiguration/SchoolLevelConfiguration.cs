using Evaluation.DAL.Entities.Org;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration;

public class SchoolLevelConfiguration : IEntityTypeConfiguration<SchoolLevel>
{
    public void Configure(EntityTypeBuilder<SchoolLevel> builder)
    {
        builder
            .HasOne(x => x.School)
            .WithMany()
            .HasForeignKey(x => x.SchoolId);
        builder
            .HasOne(x => x.EducationLevel)
            .WithMany()
            .HasForeignKey(x => x.EducationLevelId);

    }
}
