using Evaluation.DAL.Entities.OrganizationTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class SchoolLevelConfiguration : IEntityTypeConfiguration<SchoolLevel>
{
    public void Configure(EntityTypeBuilder<SchoolLevel> builder)
    {
        builder
            .HasOne(x => x.School)
            .WithMany()
            .HasForeignKey(x => x.School);

        builder
            .HasOne(x => x.Level)
            .WithMany()
            .HasForeignKey(x => x.LevelId);
    }
}
