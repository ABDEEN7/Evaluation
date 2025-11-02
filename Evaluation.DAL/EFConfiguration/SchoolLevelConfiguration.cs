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
            .HasOne(x => x.Level)
            .WithMany()
            .HasForeignKey(x => x.LevelId);

    }
}
