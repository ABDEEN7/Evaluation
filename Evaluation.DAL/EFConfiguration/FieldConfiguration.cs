using Evaluation.DAL.Entities.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evaluation.DAL.EFConfiguration
{
    public class FieldConfiguration : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.HasOne(e => e.ReadField)
                 .WithMany()
                 .HasForeignKey(e => e.ReadFieldId)
                 .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.MappingField)
                .WithMany()
                .HasForeignKey(e => e.MappingFieldId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
           .HasOne(f => f.FormGroup)
           .WithMany(fg => fg.Fields)
           .HasForeignKey(f => f.FormGroupId)
           .OnDelete(DeleteBehavior.Cascade);

            // Secondary relationship: FormGroupList -> Fields
            builder
                .HasOne(f => f.FormGroupList)
                .WithMany()
                .HasForeignKey(f => f.FormGroupListId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
