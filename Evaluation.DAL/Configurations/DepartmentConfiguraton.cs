using Evaluation.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class DepartmentConfiguraton : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder
            .HasMany<DepartmentOrganizationUnit>()
            .WithOne(x => x.Department)
            .HasForeignKey(x => x.DepartmentId);
    }
}
