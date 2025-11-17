using Evaluation.DAL.Models.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Evaluation.DAL.EFConfiguration
{

    internal class ModuleTypeConfigration : IEntityTypeConfiguration<ModuleType>
    {
        public void Configure(EntityTypeBuilder<ModuleType> builder)
        {
            builder.HasOne(x => x.Parent)
                .WithMany()
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
