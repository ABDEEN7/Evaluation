using Evaluation.DAL.Entities.FormsModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class AcademicYearScopeConfiguration : IEntityTypeConfiguration<AcademicYearScope>
{
    public void Configure(EntityTypeBuilder<AcademicYearScope> builder)
    {
        builder
            .HasOne(x => x.Department)
            .WithMany()
            .HasForeignKey(x => x.DepartmentId);

        builder
            .HasOne(x => x.Scope)
            .WithMany()
            .HasForeignKey(x => x.ScopeId);

        builder
           .HasOne(x => x.Parent)
           .WithMany()
           .HasForeignKey(x => x.ParentId);

        builder
            .HasOne(x => x.AcademicYear)
            .WithMany()
            .HasForeignKey(x => x.AcademicYearId);

    }
}
