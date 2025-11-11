using Evaluation.DAL.Helper;
using Microsoft.EntityFrameworkCore;
namespace Evaluation.DAL.Repositories

{
    public class ViewRepository<T>(DbContext context, UserInfo userInfo)
        : RepositoryBase<T>(context, userInfo)
        where T : class
{
}
}
