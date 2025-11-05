using Evaluation.DAL.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration
{
    internal class MinistryUserConfiguration : IEntityTypeConfiguration<MinistryUser>
    {
        public void Configure(EntityTypeBuilder<MinistryUser> builder)
        {
            builder.HasOne(c => c.CreateBy).WithMany().HasForeignKey(y => y.CreateById).IsRequired(false);

            builder.HasOne(c => c.UpdateBy).WithMany().HasForeignKey(c => c.UpdateById);

            builder.HasOne(c => c.DeleteBy).WithMany().HasForeignKey(c => c.DeleteById).IsRequired(false);
        }
    }
}

