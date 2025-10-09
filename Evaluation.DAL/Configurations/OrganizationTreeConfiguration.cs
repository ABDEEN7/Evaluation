using Evaluation.DAL.Entities.OrganizationTrees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class OrganizationTreeConfiguration : IEntityTypeConfiguration<OrganizationTree>
{
    public void Configure(EntityTypeBuilder<OrganizationTree> builder)
    {
        builder
            .HasOne(x => x.ParentTree)
            .WithMany()
            .HasForeignKey(x => x.ParentTreeId);

    }
}
