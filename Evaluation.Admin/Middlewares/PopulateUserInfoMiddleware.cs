using Evaluation.DAL.Entities.PermissionEntity;
using Evaluation.DAL.Entities.UserEntiy;
using Evaluation.DAL.Helper;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Admin.Middlewares
{
    public class PopulateUserInfoMiddleware
    {
        private readonly RequestDelegate _next;
        protected readonly IServiceProvider serviceProvider;
        
        public PopulateUserInfoMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            this.serviceProvider = serviceProvider;
           
        }

        public async Task InvokeAsync(HttpContext context, UserInfo userInfo)
        {

            if (context.User.Identity.IsAuthenticated)
            {
                //userInfo.UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                userInfo.Email = context.User.FindFirst("preferred_username")?.Value; // Change if necessary
                userInfo.Name=context.User.FindFirst("name")?.Value;
                using (var scope = serviceProvider.CreateScopedUow())
                {
                    if (!string.IsNullOrEmpty(userInfo.Email))
                    {

                        var _UserObj = await scope.GetRepository<UserRole>()
                                                  .GetAll()
                                                  .Include(x=>x.User)
                                                  .Where(c => c.User.Email == userInfo.Email).FirstOrDefaultAsync();
                        if (_UserObj != null)
                        {
                            userInfo.PermissionList = GetPermissions(_UserObj.RoleId);
                            userInfo.UserId = _UserObj.UserId;
                            userInfo.DBName = _UserObj.User!.NameEn; 

                        }
                    }
                }

            }

            await _next(context);


        }
       
        public List<string> GetPermissions(Guid RoleID)
        {
            try
            {
                List<string> _permission= new List<string>();
                using (var scope = serviceProvider.CreateScopedUow())
                {
                     _permission = scope.GetRepository<RolePermission>()
                                        .GetAllActiveNonDeleted()
                                        .Include(p => p.Permission)
                                        .Where(c =>c.RoleId == RoleID)
                                        .Select(x=>x.Permission!.BackendName).ToList();

                    return _permission;
                }
            }
            catch (Exception ex)
            {
                throw ex;
               
            }
        }
       

    }

}
