using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.PermissionEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvPermissionBL : AdminBase
    {
        public SrvPermissionBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }

        public async Task<List<PagePermission>> GetAllPagePermissionList()
        {
           
            var result=await uow.GetRepository<PagePermission>()
                                .GetAllNonDeleted()
                                .Include(x=>x.Page)
                                .Include(x=>x.Permission)
                                .ToListAsync();
           
            return result;

        }

        public async Task<List<RolePermissionDTO>> GetRolePermissionList(Guid roleId)
        {
          
            var result=await uow.GetRepository<RolePermission>()
                                .GetAllNonDeleted()
                                .Where(x=>x.RoleId== roleId)
                                .Select(x => new RolePermissionDTO
                                {
                                    Id = x.Id,
                                    RoleId = x.RoleId,
                                    PermissionId = x.PermissionId,
                                    IsActive = x.IsActive
                                })
                                .AsNoTracking()
                                .ToListAsync();
           
            return result;

        }
        public async Task<List<RolePermissionDTO>> SaveRolePermission(List<RolePermissionDTO> message)
        {
            try
            {
                if (message.Count > 0)
                {
                    var RoleId=message[0].RoleId;


                    List<RolePermission>  objentity = await uow.GetRepository<RolePermission>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.RoleId == RoleId)
                                      .ToListAsync();
                    List<RolePermission>  updateentity=new List<RolePermission>();

                    if (objentity.Count > 0)
                    {
                        foreach (var item in objentity)
                        {
                            bool exists = message.Any(x => x.PermissionId == item.PermissionId);
                            if (exists)
                            {
                                item.IsActive = true;
                                updateentity.Add(item);

                            }
                            else
                            {
                                uow.GetRepository<RolePermission>().Delete(item);
                            }

                        }
                    }

                    if (updateentity.Count > 0)
                    {
                        uow.GetRepository<RolePermission>().UpdateRange(updateentity);
                    }
                    if (message.Count > 0)
                    {
                        var notInSelected = message.Select(x=>x.PermissionId!.Value).Except(updateentity.Select(x=>x.PermissionId)).ToList();
                        List<RolePermission> objentitylist=new List<RolePermission>();
                        foreach (var item in notInSelected)
                        {
                            RolePermission objentityinsert = new RolePermission();
                            objentityinsert.RoleId = RoleId!.Value;
                            objentityinsert.PermissionId = item;
                            objentityinsert.IsActive = true;
                            objentitylist.Add(objentityinsert);
                        }
                        if (objentitylist.Count > 0)
                        {
                            await uow.GetRepository<RolePermission>().InsertRange(objentitylist);
                        }


                    }
                    await uow.CommitAsync();
                    message[0].ResponseStatus = DBResult.Updated;
                }
            }
            catch(Exception)
            {
                throw;
            }

          
            return message;

        }

    }
}
