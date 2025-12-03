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
    public class SrvRoleBL : AdminBase
    {
        public SrvRoleBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<RoleDTO>> GetRoleList(int Page, int PageSize)
        {
           

            
            var mapper = await CreateMapperForAdmin<Role, RoleDTO>();

            var list = await uow.GetRepository<Role>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<RoleDTO>>(list);
           
            return result;
           
        }

        public async Task<RoleDTO> SaveRole(RoleDTO message, Guid? CloneRole)
        {
           
            var mapper = await CreateMapperForAdmin<Role, RoleDTO>();

            Role obj = new Role
            {
                NameAr = message.NameAr,
                NameEn = message.NameEn,
                IsActive = message.IsActive
            };

            uow.GetRepository<Role>().Insert(obj);
            //insert values to RolePermission If CloneRoleId 
            if (CloneRole is not null)
            {
                var CloneRoleData=await uow.GetRepository<RolePermission>()
                    .GetAllNonDeleted()
                    .Where(x=>x.RoleId==CloneRole)
                    .Select(x=>new RolePermission
                    {
                        RoleId=obj.Id,
                        PermissionId=x.PermissionId,
                        IsActive=x.IsActive
                    }).ToListAsync();
                if(CloneRoleData.Count>0)
                {
                    await uow.GetRepository<RolePermission>().InsertRange(CloneRoleData);
                }
            }

            await uow.CommitAsync();
                var result = mapper.Map<RoleDTO>(obj);
            result.UpdateBy = userInfo.DBName;
            result.ResponseStatus = DBResult.Inserted;
                return result;
            
        }
        public async Task<RoleDTO> UpdateRole(RoleDTO message)
        {
           
            
          
                var mapper = await CreateMapperForAdmin<Role, RoleDTO>();
                var result = new RoleDTO();

            if (message.Id is not null)
            {


                Role obj = await uow.GetRepository<Role>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.IsActive = message.IsActive;

                uow.GetRepository<Role>().Update(obj);

                await uow.CommitAsync();
                    result = mapper.Map<RoleDTO>(obj);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
                }

                return result;
           
        }
        
        public async Task<RoleDTO> DeleteRole(Guid? Id)
        {

           

                var mapper = await CreateMapperForAdmin<Role, RoleDTO>();
                var result = new RoleDTO();
                if (Id is not null)
                {
                var objcount = uow.GetRepository<UserRole>()
                                                .GetAllNonDeleted(x => x.RoleId ==Id).Count();
                if(objcount > 0)
                {
                    result.ResponseStatus = DBResult.Exist;
                    return result;
                }

                var objlist =await uow.GetRepository<RolePermission>()
                                 .GetAllNonDeleted(x => x.RoleId ==Id)
                                 .ToListAsync();
                if(objlist.Count>0)
                {
                    foreach(var item in objlist)
                    {
                        uow.GetRepository<RolePermission>().Delete(item);
                    }
                }

                Role obj = await uow.GetRepository<Role>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                    uow.GetRepository<Role>().Delete(obj);
                    await uow.CommitAsync();
                    result = mapper.Map<RoleDTO>(obj);
                    result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
