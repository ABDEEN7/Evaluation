using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void AddGlobalQueryFilter<TEntity>(ModelBuilder modelBuilder, Type entityType)
            where TEntity : EntityBase
        {

            Expression<Func<TEntity, bool>> filter = e => e.IsDeleted == false && e.IsActive == true;
            modelBuilder.Entity<TEntity>().HasQueryFilter(filter);
        }
    }
}
