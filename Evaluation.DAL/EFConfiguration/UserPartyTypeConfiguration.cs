using Evaluation.DAL.Models.UserEntiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration
{
    internal class UserPartyTypeConfiguration : IEntityTypeConfiguration<UserPartyType>
    {
        public void Configure(EntityTypeBuilder<UserPartyType> builder)
        {
            builder.HasOne(up => up.User)
     .WithMany(u => u.UserPartTypes)
     .HasForeignKey(up => up.UserId)
     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
