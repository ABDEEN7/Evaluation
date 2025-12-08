using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.PermissionEntity;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvUserPermissionBL : AdminBase
    {
        public SrvUserPermissionBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }
        public async Task<List<UserProfileDTO>> GetAllUserList(AdminSearchDTO message)
        {
           
          

            var list = await uow.GetRepository<UserRole>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .Include(x => x.User)
                .OrderByDescending(x=>x.CreateDate)
                .Select(x=>new UserProfileDTO
                {
                    Id=x.User!.Id,
                    QID=x.User.QID,
                    Email=x.User.Email,
                    NameAr=x.User.NameAr,
                    NameEn=x.User.NameEn,
                    JobTitleAr=x.User.JobTitleAr,
                    JobTitleEn=x.User.JobTitleEn,
                    Role=(_requestInfo.Lang=="ar"?x.Role!.NameAr:x.Role!.NameEn),
                    RoleId=x.RoleId,
                    IsActive=x.User.IsActive,
                    UpdateBy=(x.User.UpdateBy != null?(_requestInfo.Lang == "ar" ?x.User.UpdateBy.NameAr: x.User.UpdateBy.NameEn):(_requestInfo.Lang == "ar" ?x.User.CreateBy!.NameAr: x.User.CreateBy!.NameEn)),
                    UpdateDate=(x.User.UpdateDate.HasValue ? x.User.UpdateDate.Value.ToString() : x.User.CreateDate.ToString()),
                })
                .ToListAsync();
            if(!string.IsNullOrEmpty(message.Title))
            {
                list = list.Where(x =>
    !string.IsNullOrEmpty(x.NameAr) && x.NameAr.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(x.NameEn) && x.NameEn.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0
).ToList();

            }
            if (!string.IsNullOrEmpty(message.Email))
            {
                list = list.Where(x =>
     !string.IsNullOrEmpty(x.Email) && x.Email.IndexOf(message.Email, StringComparison.OrdinalIgnoreCase) >= 0
 ).ToList();

            }
            if (message.RoleId is not null)
            {
                list = list.Where(x => x.RoleId == message.RoleId).ToList();
            }
            if (!string.IsNullOrEmpty(message.IsActive))
            {
                var active=(message.IsActive=="1"?true:false);
                list = list.Where(x => x.IsActive == active).ToList();
            }
            list = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();
            var result = mapper.Map<List<UserProfileDTO>>(list);

            return result;
            
        }
        public async Task<List<Role>> GetRoleList()
        {


         
            var result = await uow.GetRepository<Role>()
               .GetAllNonDeleted()
               .Include(x => x.CreateBy)
               .OrderByDescending(x => x.CreateDate)
               .ToListAsync();

            return result;

        }

       
        public async Task<UserProfileDTO> SaveUser(UserProfileDTO message)
        {
           
                if (!string.IsNullOrEmpty(message.Email))
                {
                    var checkexist = await uow.GetRepository<MinistryUser>()
                                         .GetAllNonDeleted()
                                         .Where(x => x.Email.ToLower() == message.Email.ToLower())

                                         .ToListAsync();

                    if (checkexist.Count > 0)
                    {
                        message.ResponseStatus = DBResult.Exist;
                        return message;
                    }
                }
            if (!string.IsNullOrEmpty(message.QID))
            {
                message.QID = "M_" + message.QID;
                var checkexist = await uow.GetRepository<MinistryUser>()
                                         .GetAllNonDeleted()
                                         .Where(x => x.QID == message.QID)

                                         .ToListAsync();

                if (checkexist.Count > 0)
                {
                    message.ResponseStatus = DBResult.HaveRecords;
                    return message;
                }
            }


            MinistryUser obj = new MinistryUser();


                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.JobTitleAr = message.JobTitleAr;
                obj.JobTitleEn = message.JobTitleEn;
                obj.QID = message.QID;
                obj.Email = message.Email.ToLower();
                obj.PreferredLanguage = "en";
                obj.IsActive = message.IsActive;
                uow.GetRepository<MinistryUser>().Insert(obj);

                UserRole objrole = new UserRole();
                objrole.RoleId = message.RoleId;
                objrole.UserId = obj.Id;
                objrole.IsActive = true;
                uow.GetRepository<UserRole>().Insert(objrole);
                
                await uow.CommitAsync();
                message.Id = obj.Id;
            message.Role = await uow.GetRepository<Role>().GetAllNonDeleted().Where(x => x.Id == message.RoleId).Select(x => _requestInfo.Lang == "ar" ? x.NameAr : x.NameEn).FirstOrDefaultAsync()??string.Empty;
                message.CreateBy = userInfo.DBName;
                message.ResponseStatus = DBResult.Inserted;
                message.UpdateDate = obj.CreateDate.ToString();
                return message;
           
            
        }
        public async Task<UserProfileDTO> UpdateUser(UserProfileDTO message)
        {

            if (message.Id is not null)
            {
                MinistryUser? obj = await uow.GetRepository<MinistryUser>().GetAllNonDeleted().Where(x=>x.Id==message.Id).FirstOrDefaultAsync();
                
                if(obj!=null)
                { 
                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                    obj.JobTitleAr = message.JobTitleAr;
                    obj.JobTitleEn = message.JobTitleEn;
                    obj.QID = obj.QID;
                obj.Email = obj.Email;
                obj.IsActive = message.IsActive;

                uow.GetRepository<MinistryUser>().Update(obj);
                }
                if (message.Id is not null && obj != null)
                {
                    UserRole? objuserrole = await       uow.GetRepository<UserRole>()
                        .GetAllNonDeleted()
                        .Where(x=>x.UserId==obj.Id)
                        .FirstOrDefaultAsync();
                    if(objuserrole!=null)
                    {
                        if (objuserrole.RoleId != message.RoleId)
                        {
                            uow.GetRepository<UserRole>().Delete(objuserrole);
                            UserRole objrole = new UserRole();
                            objrole.RoleId = message.RoleId;
                            objrole.UserId = obj.Id;
                            objrole.IsActive = true;
                            uow.GetRepository<UserRole>().Insert(objrole);
                           
                        }
                       
                    }
                    message.UpdateDate = obj.UpdateDate.ToString();

                }

                
            }


            
            await uow.CommitAsync();
            message.Role = await uow.GetRepository<Role>().GetAllNonDeleted().Where(x => x.Id == message.RoleId).Select(x => _requestInfo.Lang == "ar" ? x.NameAr : x.NameEn).FirstOrDefaultAsync()??string.Empty;
            message.UpdateBy = userInfo.DBName;
            message.ResponseStatus = DBResult.Updated;
           
            return message;

        }
     
     
       
    }
}
