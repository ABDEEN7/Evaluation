using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.UserEntiy;
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
    public class SrvSiteContentBL : AdminBase
    {
        public SrvSiteContentBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }
        public async Task<List<SiteContentDTO>> GetSiteContentList(AdminSearchDTO message)
        {


           
            var list = await uow.GetRepository<SiteContent>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x=>x.OrderNo)
                .ThenByDescending(x=>x.CreateDate)
                .ToListAsync();
            if(!string.IsNullOrEmpty(message.UserName))
            {
                list = list.Where(x =>
     (x.CreateBy?.NameAr != null && x.CreateBy.NameAr.IndexOf(message.UserName, StringComparison.OrdinalIgnoreCase) >= 0) ||
     (x.CreateBy?.NameEn != null && x.CreateBy.NameEn.IndexOf(message.UserName, StringComparison.OrdinalIgnoreCase) >= 0) ||
     (x.UpdateBy?.NameAr != null && x.UpdateBy.NameAr.IndexOf(message.UserName, StringComparison.OrdinalIgnoreCase) >= 0) ||
     (x.UpdateBy?.NameEn != null && x.UpdateBy.NameEn.IndexOf(message.UserName, StringComparison.OrdinalIgnoreCase) >= 0)
 ).ToList();

            }
            if (message.navbarId is not null)
            {
                list = list.Where(c => c.NavbarId == message.navbarId).ToList();
            }
            if (!string.IsNullOrEmpty(message.Title))
            {
                list = list.Where(c =>
    !string.IsNullOrEmpty(c.TitleAr) && c.TitleAr.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.TitleEn) && c.TitleEn.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.Routing) && c.Routing.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0
).ToList();

            }
            list = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();
            var result = mapper.Map<List<SiteContentDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;
            
        }
        public async Task<List<SiteContentDTO>> GetRawSiteContentList(bool includeSubContent = false)
        {


            var mapper = await CreateMapperForAdmin<SiteContent, SiteContentDTO>();
            var list = await uow.GetRepository<SiteContent>()
               .GetAllNonDeleted()
               .Include(x => x.CreateBy)
               .Where(x => includeSubContent || x.ParentId == null)
               .OrderBy(x => x.OrderNo)
               .ThenByDescending(x => x.CreateDate)
               .ToListAsync();
            var result = mapper.Map<List<SiteContentDTO>>(list);

            return result;

        }

        public async Task<List<NavbarDTO>> GetNavbarList(string Lang = "ar")
        {
            var mapper = await CreateMapperForAdmin<Navbar, NavbarDTO>();

            var list = await uow.GetRepository<Navbar>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                .ToListAsync();


            var result = mapper.Map<List<NavbarDTO>>(list);

            return result;
        }
        public async Task<List<NavbarDTO>> GetNavbarListFromSiteContent()
        {
            var mapper = await CreateMapperForAdmin<Navbar, NavbarDTO>();

            var list = await uow.GetRepository<Navbar>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                //.Include(x => x.SiteContent)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                .ToListAsync();


            var result = mapper.Map<List<NavbarDTO>>(list);

            return result;


        }
        public async Task<SiteContentDTO> SaveSiteContent(SiteContentDTO message)
        {
           
            var navBarId = message.NavbarId;
          
            if (!message.Routing.StartsWith("/Content/"))
                message.Routing = "/Content/" + message.Routing;
            var objcount =  uow.GetRepository<SiteContent>()
                                       .GetAllActiveNonDeleted()
                                       .Where(x=>x.Routing==message.Routing)
                                       .Count();
            if(objcount>0)
            {
                message.ResponseStatus = DBResult.Exist;
                return message;
            }
            if (message.ParentId is not null)
            {
                var maxSubContentCount = await uow.GetRepository<SystemSetting>()
                                         .GetAllNonDeleted()
                                         .Where(x => x.SettingKey == ConstantKeys.AdminSettings.SubSiteContentMaxCount)
                                         .Select(x => x.SettingValue)
                                         .FirstOrDefaultAsync();
                var currentParentChildren = uow.GetRepository<SiteContent>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.ParentId == message.ParentId)
                                      .Count();
                var parentContent = await uow.GetRepository<SiteContent>()
                                    .GetAllActiveNonDeleted()
                                    .Where(x => x.Id == message.ParentId)
                                    .FirstOrDefaultAsync();              

                if (parentContent != null && parentContent.NavbarId != null)
                {
                    navBarId = parentContent.NavbarId;
                }

                if (maxSubContentCount!=null && currentParentChildren >= int.Parse(maxSubContentCount))
                {
                    message.ResponseStatus = DBResult.ExceedRecord;
                    return message;
                }
            }
            if (message.ParentId != null)
            {
                var parentexist = await uow.GetRepository<SiteContent>()
                      .GetAllNonDeleted()
                      .Where(x=>x.Id==message.ParentId)
                      .AnyAsync();
                if (!parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                }
            }

            SiteContent obj = new SiteContent();
                if (message.FileNameAr != null)
                {
                    obj.FileNameAr = message.FileNameAr;
                    obj.FileNameAr_UiFileName = message.FileNameAr_UiFileName;
                    obj.FileNameAr_BlobURL = message.FileNameAr_BlobURL;
                    obj.FileNameAr_Size = message.FileNameAr_Size;
                    obj.FileNameAr_FileExt = message.FileNameAr_FileExt;
                }
                if (message.FileNameEn != null)
                {
                    obj.FileNameEn = message.FileNameEn;
                    obj.FileNameEn_UiFileName = message.FileNameEn_UiFileName;
                    obj.FileNameEn_BlobURL = message.FileNameEn_BlobURL;
                    obj.FileNameEn_Size = message.FileNameEn_Size;
                    obj.FileNameEn_FileExt = message.FileNameEn_FileExt;
                }

                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.ParentId = message.ParentId;
                obj.DescriptionEn = message.DescriptionEn;
                obj.DescriptionAr = message.DescriptionAr;
                obj.Routing = message.Routing;
                obj.NavbarId = navBarId;
                obj.IsActive = message.IsActive;
                uow.GetRepository<SiteContent>().Insert(obj);
            //insert values to SiteContentFaq
           

            await uow.CommitAsync();
            var result = mapper.Map<SiteContentDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Inserted;
                return result;
            
        }
        public async Task<SiteContentDTO> UpdateSiteContent(SiteContentDTO message)
        {
          
            if (!message.Routing.StartsWith("/Content/"))
                message.Routing = "/Content/" + message.Routing;
            var result = new SiteContentDTO();

           


            if (message.Id is not null)
                {
                SiteContent? currentObj = await uow.GetRepository<SiteContent>()
                                        .GetAllNonDeleted()
                                        .Where(x =>  x.Id==message.Id)
                                        .FirstOrDefaultAsync();
                if (currentObj != null)
                {
                    SiteContent? objWithSameRoute = await uow.GetRepository<SiteContent>()
                                        .GetAllNonDeleted()
                                        .Where(x => x.Routing == message.Routing && x.Id !=message.Id)
                                        .FirstOrDefaultAsync();

                    if (objWithSameRoute != null)
                    {
                        message.ResponseStatus = DBResult.Exist;
                        return message;

                    }
                    List<SiteContent> children = await uow.GetRepository<SiteContent>()
                                        .GetAllNonDeleted()
                                        .Where(x => x.ParentId == message.Id)
                                        .ToListAsync();
                    if (message.ParentId is not null)
                    {
                        if (currentObj.ParentId is null)
                        {


                            if (children.Any())
                            {
                                message.ResponseStatus = DBResult.CannotBeParent;
                                return message;

                            }
                        }
                        var maxSubContentCount = await uow.GetRepository<SystemSetting>()
                                                 .GetAllNonDeleted()
                                                 .Where(x => x.SettingKey == ConstantKeys.AdminSettings.SubSiteContentMaxCount)
                                                 .Select(x => x.SettingValue)
                                                 .FirstOrDefaultAsync();
                        var currentParentChildren = uow.GetRepository<SiteContent>()
                                              .GetAllNonDeleted()
                                              .Where(x => x.ParentId == message.ParentId && x.Id!=currentObj.Id)
                                              .Count();

                        if (maxSubContentCount != null && currentParentChildren >= int.Parse(maxSubContentCount))
                        {
                            message.ResponseStatus = DBResult.ExceedRecord;
                            return message;
                        }
                    }
                    if (message.ParentId == currentObj.Id)
                    {
                        result.ResponseStatus = DBResult.SameRecord;
                        return result;
                    }
                    if (message.ParentId != null)
                    {
                        var parentexist = await uow.GetRepository<SiteContent>()
                      .GetAllNonDeleted()
                      .Where(x=>x.Id==message.ParentId)
                      .AnyAsync();
                        if (!parentexist)
                        {
                            throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                        }
                    }
                    if (message.FileNameAr != null)
                    {
                        currentObj.FileNameAr = message.FileNameAr;
                        currentObj.FileNameAr_UiFileName = message.FileNameAr_UiFileName;
                        currentObj.FileNameAr_BlobURL = message.FileNameAr_BlobURL;
                        currentObj.FileNameAr_Size = message.FileNameAr_Size;
                        currentObj.FileNameAr_FileExt = message.FileNameAr_FileExt;
                    }
                    if (message.FileNameEn != null)
                    {
                        currentObj.FileNameEn = message.FileNameEn;
                        currentObj.FileNameEn_UiFileName = message.FileNameEn_UiFileName;
                        currentObj.FileNameEn_BlobURL = message.FileNameEn_BlobURL;
                        currentObj.FileNameEn_Size = message.FileNameEn_Size;
                        currentObj.FileNameEn_FileExt = message.FileNameEn_FileExt;
                    }
                    currentObj.TitleAr = message.TitleAr;
                    currentObj.TitleEn = message.TitleEn;
                    currentObj.Routing = message.Routing;
                    currentObj.NavbarId = message.NavbarId;
                    currentObj.ParentId = message.ParentId;
                    currentObj.DescriptionAr = message.DescriptionAr;
                    currentObj.DescriptionEn = message.DescriptionEn;
                    currentObj.IsActive = message.IsActive;
                    uow.GetRepository<SiteContent>().Update(currentObj);
                 
                    await uow.CommitAsync();
                    result = mapper.Map<SiteContentDTO>(currentObj, opts => opts.Items["Language"] = _requestInfo.Lang);
                    result.ResponseStatus = DBResult.Updated;
                }
                   
                }

                return result;
           
        }
        public async Task<bool> UpdateSiteContentOrder(List<OrderingDTO> message)
        {
            bool rtn = false;
           
                var updatedRows = from updatedItem in message
                                  join rowToUpdate in uow.GetRepository<SiteContent>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                                  select new { Row = rowToUpdate, updatedItem.OrderNo };



                updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

                await uow.CommitAsync();
                rtn = true;

           
            return rtn;
        }
        public async Task<SiteContentDTO> DeleteSiteContent(Guid? Id)
        {

            
               
                var result = new SiteContentDTO();
                if (Id is not null)
                {
                    SiteContent obj = await uow.GetRepository<SiteContent>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                var parentexist = await uow.GetRepository<SiteContent>()
          .GetAllNonDeleted()
          .Where(x=>x.ParentId==obj.Id)
          .AnyAsync();
                if (parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.CannotDeleteItsParent);
                }
                uow.GetRepository<SiteContent>().Delete(obj);
                    await uow.CommitAsync();
                    result = mapper.Map<SiteContentDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }

       
    }
}
