using Evaluation.DAL.Entities.UserEntiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Evaluation.DAL.EFConfiguration
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {

            builder.HasOne(up => up.User)
                 .WithMany(u => u.UserRoles)
                 .HasForeignKey(up => up.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            
        }

    }

}
