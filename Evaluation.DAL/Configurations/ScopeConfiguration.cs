using Evaluation.DAL.Entities.FormsModules;
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

       

        builder.HasOne<ScopeType>()
            .WithMany()
            .HasForeignKey(x => x.ScopeTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
