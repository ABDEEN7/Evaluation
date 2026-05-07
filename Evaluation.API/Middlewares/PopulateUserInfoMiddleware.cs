using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper;
using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.PermissionEntity;
using Evaluation.DAL.Models.UserEntiy;


namespace Evaluation.API.Middlewares
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

        //public async Task InvokeAsync(HttpContext context, UserInfo userInfo)
        //{

        //    userInfo.UserId = Guid.Parse("98786FAF-B41C-489A-AF3F-3B61FB5B079B");
        //    userInfo.Email = "test.user@moehe.gov.qa";
        //    userInfo.Name = "Test User";
        //    userInfo.UserType = "MinistryUser";
        //    userInfo.DBName = "EvaluationDB";
        //    var RoleId = Guid.Parse("ffe7ba17-0375-4616-b526-29b0989a67ec");
        //    userInfo.PartyTypes = new List<Guid>
        //    {
        //        Guid.Parse("22222222-2222-2222-2222-222222222222"),
        //        Guid.Parse("33333333-3333-3333-3333-333333333333")
        //    };
        //    userInfo.PermissionList = GetPermissions(RoleId);

        //    //userInfo.PermissionList = new List<string>
        //    //    {
        //    //        // Evaluation Plans
        //    //        "ViewEvaluationPlans",
        //    //        "CreateEvaluationPlan",
        //    //        "EditEvaluationPlan",
        //    //        "DeleteEvaluationPlan",
        //    //        "ApproveEvaluation",

        //    //        // Web Plan Permissions
        //    //        "GET_WEB_PLAN_TYPE_REQUEST",
        //    //        "ADD_WEB_PLAN_REQUEST",
        //    //        "APPROVE_WEB_PLAN_REQUEST",
        //    //        "GET_SEMESTERS_REQUEST"
        //    //    };

        //    await _next(context);
        //}



        public async Task InvokeAsync(HttpContext context, UserInfo userInfo)
        {
            using (var scope = serviceProvider.CreateScopedUow())
            {
                if (context.User.Identity!.IsAuthenticated)
                {
                    userInfo.UserId = Guid.Parse(context.User.FindFirst(UserProfileClaim.UserId.ToString())?.Value ?? "");
                    userInfo.Email = context.User.FindFirst(UserProfileClaim.Email.ToString())?.Value ?? ""; // Change if necessary
                                                                                                             //userInfo.Roles = context.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
                    userInfo.Name = context.User.FindFirst(UserProfileClaim.FullNameEn.ToString())?.Value ?? "";
                    userInfo.UserType = context.User.FindFirst(UserProfileClaim.UserType.ToString())?.Value ?? "";
                    if (!string.IsNullOrEmpty(userInfo.Email))
                    {

                        var _UserObj = await scope.GetRepository<UserRole>()
                                                  .GetAllQueryFiltered()
                                                  .Include(x => x.User)
                                                  .Include(x => x.Role)
                                                  .Where(c => c.User!.Email == userInfo.Email && c.IsActive == true && c.IsDeleted == false && c.User.IsActive == true && c.User.IsDeleted == false && c.Role!.IsActive == true && c.Role.IsDeleted == false).FirstOrDefaultAsync();
                        if (_UserObj != null)
                        {
                            userInfo.PermissionList = GetPermissions(_UserObj.RoleId);
                            userInfo.UserId = _UserObj.UserId;
                            userInfo.DBName = _UserObj.User!.NameEn;

                        }
                    }

                    if (userInfo.UserId != null)
                    {
                        var isMinistry = scope.GetRepository<MinistryUser>().GetAll(x => x.Id == userInfo.UserId).Any();
                        if (isMinistry)
                        {
                            var userPArtytypes = scope.GetRepository<UserPartyType>().GetAllQueryFiltered().Where(c => c.UserId == userInfo.UserId).Select(c => c.PartyTypeId).ToList();
                            userInfo.PartyTypes = userPArtytypes;
                        }
                        else
                        {
                            var userPArtytypes = scope.GetRepository<PartyType>().GetAllQueryFiltered()
                                .Where(c => !c.IsEmployeePartyType).Select(c => c.Id).ToList();

                            userInfo.PartyTypes = userPArtytypes;
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
                List<string> _permission = new List<string>();
                using (var scope = serviceProvider.CreateScopedUow())
                {
                    _permission = scope.GetRepository<RolePermission>()
                                       .GetAllActiveNonDeleted()
                                       .Include(p => p.Permission)
                                       .Where(c => c.RoleId == RoleID)
                                       .Select(x => x.Permission!.BackendName).ToList();

                    return _permission;
                }
            }
            catch (Exception)
            {
                throw;

            }
        }
    }
}
