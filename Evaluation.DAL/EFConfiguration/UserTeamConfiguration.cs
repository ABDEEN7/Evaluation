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
            .WithMany(x => x.UserTeams);

        builder.HasOne(x => x.Team)
            .WithMany();
    }
}
