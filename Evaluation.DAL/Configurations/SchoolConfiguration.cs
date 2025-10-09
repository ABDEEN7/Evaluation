using Evaluation.DAL.Entities.OrganizationTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.Configurations;

public class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder
            .Property(x => x.NameAr)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(x => x.NameEn)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(x => x.Address)
            .HasMaxLength(200);

        builder
            .Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Region)
            .HasMaxLength(200);

        builder
            .HasOne(x => x.SchoolType)
            .WithMany()
            .HasForeignKey(x => x.TypeId);
      
    }
}
