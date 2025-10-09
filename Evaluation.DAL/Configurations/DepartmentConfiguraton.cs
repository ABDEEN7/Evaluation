using Evaluation.DAL.Entities.OrganizationTrees;
using Evaluation.DAL.Entities.Planing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Evaluation.DAL.Configurations;

public class DepartmentConfiguraton : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {

        builder.Property(x => x.NameAr)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(x => x.NameEn)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .HasMany<DepartmentOrganizationTree>()
            .WithOne(x => x.Department)
            .HasForeignKey(x => x.DepartmentId);

        builder
            .HasOne<OrganizationType>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationTypeId);
    }
}
