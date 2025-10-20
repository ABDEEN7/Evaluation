using Evaluation.DAL.Entities.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration
{
    public class ViewConditionConfiguration : IEntityTypeConfiguration<FieldViewCondition>
    {
        public void Configure(EntityTypeBuilder<FieldViewCondition> builder)
        {

            builder.HasOne(e => e.Field)
                .WithMany(c => c.FieldViewConditions)
                .HasForeignKey(e => e.FieldId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
