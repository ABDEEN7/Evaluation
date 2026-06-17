using Evaluation.DAL.Models.Org;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration;

public class SchoolLevelConfiguration : IEntityTypeConfiguration<SchoolLevel>
{
    public void Configure(EntityTypeBuilder<SchoolLevel> builder)
    {
        builder
            .HasOne(x => x.School)
            .WithMany(x=>x.SchoolLevel)
            .HasForeignKey(x => x.SchoolId);

        builder
            .HasOne(x => x.EducationLevel)
            .WithMany(s=>s.SchoolLevels)
            .HasForeignKey(x => x.EducationLevelId);

    }
}
