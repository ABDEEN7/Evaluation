using Evaluation.DAL.Models.Planing.TeamsModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration;

public class UserTeamConfiguration : IEntityTypeConfiguration<UserTeam>
{
    public void Configure(EntityTypeBuilder<UserTeam> builder)
    {
        builder.HasOne(x => x.User)
        .WithMany(x => x.UserTeams)
        .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Team)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.CreateBy)
            .WithMany()
            .HasForeignKey(x => x.CreateById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.UpdateBy)
            .WithMany()
            .HasForeignKey(x => x.UpdateById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.DeleteBy)
            .WithMany()
            .HasForeignKey(x => x.DeleteById)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
