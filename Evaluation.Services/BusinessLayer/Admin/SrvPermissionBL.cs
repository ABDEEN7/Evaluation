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
                if (message == null || !message.Any())
                    return message;

                var roleId = message[0].RoleId!.Value;
                List<RolePermission> existingPermissions = await uow.GetRepository<RolePermission>()
                    .GetAllNonDeleted()
                    .Where(x => x.RoleId == roleId)
                    .ToListAsync();

                List<Guid> selectedPermissionIds = message
                    .Where(x => x.PermissionId.HasValue)
                    .Select(x => x.PermissionId!.Value)
                    .ToList();


                var permissionsToDelete = existingPermissions
                    .Where(x => !selectedPermissionIds.Contains(x.PermissionId))
                    .ToList();

                if (permissionsToDelete.Any())
                {
                    uow.GetRepository<RolePermission>().DeleteRange(permissionsToDelete);
                }


                var permissionsToUpdate = existingPermissions
                    .Where(x => selectedPermissionIds.Contains(x.PermissionId))
                    .ToList();

                foreach (var item in permissionsToUpdate)
                {
                    item.IsActive = true;
                }

                if (permissionsToUpdate.Any())
                {
                    uow.GetRepository<RolePermission>().UpdateRange(permissionsToUpdate);
                }

                List<Guid> existingPermissionIds = existingPermissions
                    .Select(x => x.PermissionId)
                    .ToList();

                var permissionsToInsert = selectedPermissionIds
                    .Except(existingPermissionIds)
                    .Select(permissionId => new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permissionId,
                        IsActive = true
                    })
                    .ToList();

                if (permissionsToInsert.Any())
                {
                    await uow.GetRepository<RolePermission>().InsertRange(permissionsToInsert);
                }

                await uow.CommitAsync();

                message[0].ResponseStatus = DBResult.Updated;
            }
            catch
            {
                throw;
            }

            return message;
        }
    }
}
