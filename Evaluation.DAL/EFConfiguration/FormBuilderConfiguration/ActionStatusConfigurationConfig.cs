using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Entities.ActionEntities;

namespace Evaluation.DAL.EFConfiguration.FormBuilderConfiguration
{
    internal class ActionStatusConfigurationConfig : IEntityTypeConfiguration<ActionStatusConfiguration>
    {


        public void Configure(EntityTypeBuilder<ActionStatusConfiguration> builder)
        {
            builder.HasOne(o => o.CurrentStatus)
               .WithMany(c => c.CurrentStatusConfigurations)
               .HasForeignKey(o => o.CurrentStatusId)
               .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(o => o.NextStatus)
                   .WithMany(c => c.NextStatusConfigurations)
                   .HasForeignKey(o => o.NextStatusId)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
