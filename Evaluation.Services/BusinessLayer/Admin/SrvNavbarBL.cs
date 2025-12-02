using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.PermissionEntity;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Evaluation.SharedHelper.Enums.ConstantKeys;
namespace Evaluation.Services.Models.Admin
{
    public class SrvNavbarBL : AdminBase
    {
        public SrvNavbarBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<NavbarDTO>> GetNavbarList(Guid? parentid,int Page, int PageSize)
        {


           

            Guid specialGuid = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
            if (parentid.Equals(specialGuid))
            {
                parentid = null;
            }
            var list = await uow.GetRepository<Navbar>()
                .GetAllNonDeleted()
                 .Where(x=> parentid==null?x.ParentId==null:(parentid == Guid.Empty || x.ParentId==parentid))
                .Include(x => x.CreateBy)
                 .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<NavbarDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
        public async Task<List<NavbarDTO>> GetParentNavbarListWithMater()
        {
           
            

            NavbarDTO newObj = new NavbarDTO
            {
                Id = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
                TitleAr = "الرئيسية",
                TitleEn = "Master",
                OrderNo = null,
                Parent = null,
                IsActive = true
            };
            
            var rslt = await uow.GetRepository<Navbar>()
                    .GetAllActiveNonDeleted()
                    //.Where(c => c.ParentId==null)
            .ToListAsync();

            var result = mapper.Map<List<NavbarDTO>>(rslt, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.Insert(0,newObj);
            return result.OrderBy(x => x.OrderNo).ToList();
        }
        public async Task<List<NavbarDTO>> GetNavbarListWithUpAndDownLevel()
        {
            var rslt = await uow.GetRepository<Navbar>()
                            .GetAllActiveNonDeleted()
                            .OrderBy(x => x.OrderNo)
                            .ToListAsync();

            var result = mapper.Map<List<NavbarDTO>>(rslt, opts => opts.Items["Language"] = _requestInfo.Lang);

            await result.ParallelForEachAsync(async (item) =>
            {
                try
                {
                    var current = item;
                    item.Level ??= 0;
                    item.LevelsUp ??= 0;

                    while (current.ParentId != Guid.Empty)
                    {
                        var parent = result.FirstOrDefault(n => n.Id == current.ParentId);
                        if (parent == null)
                            break;

                        item.Level++;
                        item.LevelsUp++;
                        current = parent;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {item.Id}: {ex.Message}");
                }
            });

            return result;



        }
        

        public async Task<List<SiteContentDTO>> GetRoutingList()
        {
           
           
               var result = await uow.GetRepository<SiteContent>()
                    .GetAllNonDeleted()
                    .OrderBy(x => x.OrderNo)
                    .ThenByDescending(x => x.CreateDate)
                    .Select(x=>new SiteContentDTO
                    {
                        Id=x.Id,
                        Routing=x.Routing,
                        TitleAr=x.TitleAr,
                        TitleEn=x.TitleEn,
                    })
                    .ToListAsync();

           

            return result;
        }


        public async Task<NavbarDTO> SaveNavbar(NavbarDTO message)
        {

           

            var objcount =  uow.GetRepository<Navbar>()
                                                .GetAllActiveNonDeleted(x => x.ParentId == null).Count();
                var settingcount = await uow.GetRepository<SystemSetting>()
                                                 .GetAllNonDeleted()
                                                 .Where(x => x.SettingKey == AdminSettings.NumberOfNavbars.ToString())
                                                 .Select(x => x.SettingValue)
                                                 .FirstOrDefaultAsync();

                if (message.ParentId == null && message.IsActive == true)
                {
                if(settingcount!=null)
                {
                    if (objcount + 1 > int.Parse(settingcount))
                    {

                        message.ResponseStatus = DBResult.ExceedRecord;
                        return message;
                    }
                }

                    
                }
            if (message.ParentId != null)
            {
                var parentexist = await uow.GetRepository<Navbar>()
                      .GetAllNonDeleted()
                      .Where(x=>x.Id==message.ParentId)
                      .AnyAsync();
                if (!parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                }
            }
            Navbar obj = new Navbar();

                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.IsInternal = message.IsInternal;
                obj.UrlEn = message.UrlEn;
                obj.UrlAr = message.UrlAr;
                obj.ParentId = message.ParentId;
                obj.Target = message.Target;
                obj.IsActive = message.IsActive;
                obj.IsAuthorized = message.IsAuthorized;
            if(message.IsAuthorized)
            {
                var Permission = await uow.GetRepository<Permission>()
                      .GetAllNonDeleted()
                      .Where(x=>x.BackendName==message.PermissionId)
                      .FirstOrDefaultAsync();
                if(Permission!=null)
                {
                    obj.PermissionId = Permission.Id;
                }
                
            }
                uow.GetRepository<Navbar>().Insert(obj);
                await uow.CommitAsync();
            var result = mapper.Map<NavbarDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
            
        }
        public async Task<NavbarDTO> UpdateNavbar(NavbarDTO message)
        {


          

            var result = new NavbarDTO();

                if (message.Id is not null)
                {
                    var objcount = uow.GetRepository<Navbar>()
                                                .GetAllActiveNonDeleted(x => x.ParentId == null && x.Id!=message.Id).Count();
                    var settingcount = await uow.GetRepository<SystemSetting>()
                                                     .GetAllNonDeleted()
                                                     .Where(x => x.SettingKey == AdminSettings.NumberOfNavbars.ToString())
                                                     .Select(x => x.SettingValue)
                                                     .FirstOrDefaultAsync();

                    if (message.ParentId == null && message.IsActive == true)
                    {
                    if(settingcount!=null)
                    {
                        if (objcount + 1 > int.Parse(settingcount))
                        {

                            message.ResponseStatus = DBResult.ExceedRecord;
                            return message;
                        }
                    }
                       
                    }

                    Navbar obj = await uow.GetRepository<Navbar>()
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
                if(message.ParentId!=null)
                {
                    var parentexist = await uow.GetRepository<Navbar>()
                      .GetAllNonDeleted()
                      .Where(x=>x.Id==message.ParentId)
                      .AnyAsync();
                    if (!parentexist)
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                    }
                }
                
                obj.TitleAr = message.TitleAr;
                    obj.TitleEn = message.TitleEn;
                    obj.IsInternal = message.IsInternal;
                    obj.ParentId = message.ParentId;
                    obj.Target = message.Target;
                    obj.UrlAr = message.UrlAr;
                    obj.UrlEn = message.UrlEn;
                    obj.IsActive = message.IsActive;
                obj.IsAuthorized = message.IsAuthorized;
                if (message.IsAuthorized)
                {
                    var Permission = await uow.GetRepository<Permission>()
                      .GetAllNonDeleted()
                      .Where(x=>x.BackendName==message.PermissionId)
                      .FirstOrDefaultAsync();
                    if (Permission != null)
                    {
                        obj.PermissionId = Permission.Id;
                    }

                }
                uow.GetRepository<Navbar>().Update(obj);
                    await uow.CommitAsync();
                 result = mapper.Map<NavbarDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
                }

                return result;
           
        }
        public async Task<bool> UpdateNavbarOrder(List<OrderingDTO> message)
        {
            bool rtn = false;
           
                var updatedRows = from updatedItem in message
                                  join rowToUpdate in uow.GetRepository<Navbar>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                                  select new { Row = rowToUpdate, updatedItem.OrderNo };



                updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

                await uow.CommitAsync();
                rtn = true;

            
            return rtn;
        }
        public async Task<NavbarDTO> DeleteNavbar(Guid? Id)
        {


           

            var result = new NavbarDTO();
                if (Id is not null)
                {
                    Navbar obj = await uow.GetRepository<Navbar>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                var parentexist = await uow.GetRepository<Navbar>()
          .GetAllNonDeleted()
          .Where(x=>x.ParentId==obj.Id)
          .AnyAsync();
                if (parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.CannotDeleteItsParent);
                }
                var SiteContent = await uow.GetRepository<SiteContent>()
.GetAllNonDeleted()
                      .Where(x => x.NavbarId == obj.Id)
                      .ToListAsync();
                if (SiteContent.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.NavbarExistsSiteContent);
                }
                uow.GetRepository<Navbar>().Delete(obj);
                    await uow.CommitAsync();
                 result = mapper.Map<NavbarDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
