using Evaluation.DAL.Entities;
using Evaluation.DAL.Entities.Planing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class ScopeConfiguration : IEntityTypeConfiguration<Scope>
{
    public void Configure(EntityTypeBuilder<Scope> builder)
    {
        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Scope>()
            .WithMany()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
