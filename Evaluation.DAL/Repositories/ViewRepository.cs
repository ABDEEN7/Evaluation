using Evaluation.SharedHelper.Helper;
using Microsoft.EntityFrameworkCore;
using System;
namespace Evaluation.DAL.Repositories

{
    public class ViewRepository<T>(DbContext context, UserInfo userInfo)
        : RepositoryBase<T>(context, userInfo)
        where T : class
{
}
}
