using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.PermissionEntity;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvSideBarBL : AdminBase
    {
        public SrvSideBarBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }

        public async Task<List<SideBarDTO>> GetMenuSideBarList()
        {
            var permissionids = userInfo.PermissionList ?? [];
            List<SideBarDTO> rslt = new List<SideBarDTO>();
            var allData = await uow.GetRepository<SideBar>()
                .GetAllNonDeleted()
                .Include(x => x.Permission)
                //.Where(x => (x.Permission != null && permissionids.Contains(x.Permission.BackendName)) || (x.ParentId == null && x.PermissionId == null))
                .OrderBy(x => x.OrderNo).ThenByDescending(x => x.CreateDate)
                .Select(c => new SideBarDTO
                {
                    Name = _requestInfo.Lang == "ar" ? c.NameAr : c.NameEn,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                    Id = c.Id,
                    RoutingPath = c.RoutingPath,
                    OrderNo = c.OrderNo,
                    ParentId = c.ParentId,
                    Icon = c.Icon,
                    IsActive = c.IsActive ?? false,
                    PermissionId = c.PermissionId

                }).ToListAsync();

            var parents = allData.Where(x => x.ParentId == null).ToList();


            foreach (var parent in parents)
            {
                var parentDTO = GenerateSideBarDTO(allData, parent);

                rslt.Add(parentDTO);
            }
            return rslt;



        }
        private SideBarDTO GenerateSideBarDTO(List<SideBarDTO> allData, SideBarDTO sidebar)
        {
            var rslt = new SideBarDTO
            {
                Id = sidebar.Id,
                Name = _requestInfo.Lang == "ar" ? sidebar.NameAr : sidebar.NameEn,
                NameAr = sidebar.NameAr,
                NameEn = sidebar.NameEn,
                RoutingPath = sidebar.RoutingPath,
                OrderNo = sidebar.OrderNo,
                ParentId = sidebar.ParentId,
                Icon = sidebar.Icon,
                IsActive = sidebar.IsActive,
                PermissionId = sidebar.PermissionId,
                ChildList = GetChildHierarchy(allData, sidebar)
            };
            return rslt;

        }
        private List<SideBarDTO> GetChildHierarchy(List<SideBarDTO> allData, SideBarDTO parent)
        {
            var result = allData
                  .OrderBy(x => x.OrderNo)
                  .Where(x => x.ParentId == parent.Id)
                  .Select(child => GenerateSideBarDTO(allData, child)).ToList().OrderBy(c => c.OrderNo).ToList();

            return result;
        }
        public async Task<List<SideBarDTO>> GetSideBarList(Guid? parentid, int Page, int PageSize)
        {



          
            Guid specialGuid = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
            if (parentid.Equals(specialGuid))
            {
                parentid = null;
            }
            var list = await uow.GetRepository<SideBar>()
                .GetAllNonDeleted()
                .Where(x=> parentid==null?x.ParentId==null:(parentid == Guid.Empty || x.ParentId==parentid))
                .Include(x => x.CreateBy)
                 .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<SideBarDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
        public async Task<List<Permission>> GetViewPermissionList()
        {


            var result = await uow.GetRepository<Permission>()
                    .GetAllNonDeleted()
                    .Where(c => c.BackendName.StartsWith("VIEW"))
                    .OrderBy(x => x.BackendName)
                    .ToListAsync();


            return result.ToList();
        }
        public async Task<List<SideBarDTO>> GetAllSideBarList()
        {

            SideBarDTO newObj = new SideBarDTO
            {
                Id = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
                NameAr = "الرئيسية",
                NameEn = "Master",
                Parent = null,
                IsActive = true
            };

           

            var list = await uow.GetRepository<SideBar>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                 .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                .ToListAsync();

            var result = mapper.Map<List<SideBarDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.Insert(0, newObj);
            return result.OrderBy(x => x.OrderNo).ToList();


        }


        public async Task<SideBarDTO> SaveSideBar(SideBarDTO message)
        {

           
            if (message.ParentId != null)
            {
                var parentexist = await uow.GetRepository<SideBar>()
          .GetAllNonDeleted()
          .Where(x=>x.Id==message.ParentId)
          .AnyAsync();
                if (!parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                }
            }

            SideBar obj = new SideBar();

            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.ParentId = (message.ParentId == Guid.Empty ? null : message.ParentId);
            obj.Icon = message.Icon;
            obj.PermissionId = message.PermissionId;
            obj.RoutingPath = message.RoutingPath;
            obj.IsActive = message.IsActive;
            uow.GetRepository<SideBar>().Insert(obj);
            await uow.CommitAsync();
            var result = mapper.Map<SideBarDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;

        }
        public async Task<SideBarDTO> UpdateSideBar(SideBarDTO message)
        {



           
            var result = new SideBarDTO();

            if (message.Id is not null)
            {

                SideBar obj = await uow.GetRepository<SideBar>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Include(x => x.Parent)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
                if (message.ParentId == obj.Id)
                {
                    result.ResponseStatus = DBResult.SameRecord;
                    return result;
                }
                if (message.ParentId != null)
                {
                    var parentexist = await uow.GetRepository<SideBar>()
          .GetAllNonDeleted()
          .Where(x=>x.Id==message.ParentId)
          .AnyAsync();
                    if (!parentexist)
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                    }
                }
                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.ParentId = (message.ParentId == Guid.Empty ? null : message.ParentId);
                obj.Icon = message.Icon;
                obj.PermissionId = message.PermissionId;
                obj.RoutingPath = message.RoutingPath;
                obj.IsActive = message.IsActive;
                uow.GetRepository<SideBar>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<SideBarDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }
        public async Task<bool> UpdateSideBarOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<SideBar>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }
        public async Task<SideBarDTO> DeleteSideBar(Guid? Id)
        {



           
            var result = new SideBarDTO();
            if (Id is not null)
            {
                SideBar obj = await uow.GetRepository<SideBar>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                var parentexist = await uow.GetRepository<SideBar>()
                .GetAllNonDeleted()
          .Where(x=>x.ParentId==obj.Id)
          .AnyAsync();
                if (parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.CannotDeleteItsParent);
                }
                uow.GetRepository<SideBar>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<SideBarDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;


        }

    }
}
