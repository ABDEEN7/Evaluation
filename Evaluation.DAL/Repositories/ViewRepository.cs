using Evaluation.DAL.Models.Base;
using Evaluation.SharedHelper.Helper;
using Microsoft.EntityFrameworkCore;
using System;
namespace Scholarship.DAL.Repositories

{
    public class ViewRepository<T>(DbContext context, UserInfo userInfo)
        : RepositoryBase<T>(context, userInfo)
        where T : class, IViewEntity<Guid>
{
}
}
