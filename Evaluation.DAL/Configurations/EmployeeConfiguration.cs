using Evaluation.DAL.Entities.OrganizationTrees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder
            .Property(x => x.EmployeeNo)
            .HasMaxLength(100);

        builder
            .Property(x => x.Gender)
            .HasMaxLength(20);

        builder
            .Property(x => x.Gender)
            .HasMaxLength(150);

        builder.HasQueryFilter(e =>
     e.BirthDate <= DateTime.UtcNow &&
     e.JoinDate <= DateTime.UtcNow &&
     e.JoinDate >= e.BirthDate.AddYears(16));

    }
}
