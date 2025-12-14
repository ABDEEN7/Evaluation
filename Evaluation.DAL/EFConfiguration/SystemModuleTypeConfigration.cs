using Evaluation.DAL.Models.DepartementEntites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.EFConfiguration
{
    internal class SystemModuleTypeConfigration : IEntityTypeConfiguration<SystemModuleType>
    {
        public void Configure(EntityTypeBuilder<SystemModuleType> builder)
        {
            //builder.HasOne(x => x.ParentModuleType)
            //    .WithMany()
            //    .HasForeignKey(x => x.ParentModuleTypeId)
            //    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
