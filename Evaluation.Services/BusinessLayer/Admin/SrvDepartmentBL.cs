using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Drawing;
using Xceed.Document.NET;
using static Evaluation.SharedHelper.Enums.ConstantKeys;
namespace Evaluation.Services.Models.Admin
{
    public class SrvDepartmentBL : AdminBase
    {
        public SrvDepartmentBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<DepartmentDTO>> GetDepartmentList(int Page, int PageSize)
        {


            var list = await uow.GetRepository<Department>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<DepartmentDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }

        public async Task<DepartmentDTO> SaveDepartment(
        DepartmentDTO message,
        List<WebsiteAttachmentDTO>? filemodel)
        {
            var backendName = await GenerateBackendNameByTitle(message.NameEn);

            var existBackendName = await uow
                .GetRepository<Department>()
                .GetAllNonDeleted(x => x.BackendName == backendName)
                .FirstOrDefaultAsync();

            if (existBackendName != null)
            {
                message.ResponseStatus = DBResult.BackendExist;
                return message;
            }

            #region Attachments

            if (filemodel != null && filemodel.Count > 0)
            {
                foreach (var item in filemodel)
                {
                    var attachment = new WebsiteAttachment
                    {
                        FileName = item.FileName,
                        UiFileName = item.UiFileName,
                        BlobUrl = item.BlobUrl,
                        FileExtension = item.FileExtension,
                        FileSize = item.FileSize,
                        IsActive = true
                    };

                    await uow.GetRepository<WebsiteAttachment>()
                        .InsertAsync(attachment);

                    switch (item.ControlFileName)
                    {
                        case "DepartmentDepImageFileNameAr":
                            message.DepImageFileNameAr_BlobURL = item.BlobUrl;
                            message.DepImageFileNameAr = item.FileName;
                            message.DepImageFileNameAr_UiFileName = item.UiFileName;
                            break;

                        case "DepartmentDepImageFileNameEn":
                            message.DepImageFileNameEn_BlobURL = item.BlobUrl;
                            message.DepImageFileNameEn = item.FileName;
                            message.DepImageFileNameEn_UiFileName = item.UiFileName;
                            break;

                        case "WebsiteAttachmentId":
                            message.WebsiteAttachmentId = attachment.Id;
                            break;
                    }
                }
            }

            #endregion

            var department = new Department
            {
                NameAr = message.NameAr,
                NameEn = message.NameEn,
                BackendName = backendName,
                RoutingPath = message.RoutingPath,
                DepIcon = message.DepIcon,
                IsEvaluated = message.IsEvaluated,
                IsNDA = message.IsNDA,
                DescAr = message.DescAr,
                DescEn = message.DescEn,
                WebsiteAttachmentId = message.WebsiteAttachmentId,
                IsActive = message.IsActive,

                DepImageFileNameAr = message.DepImageFileNameAr,
                DepImageUiFileNameAr =
                    !string.IsNullOrEmpty(message.DepImageFileNameAr_UiFileName) &&
                    message.DepImageFileNameAr_UiFileName.Length > 45
                        ? message.DepImageFileNameAr_UiFileName.Substring(0, 40) +
                          message.DepImageFileNameAr_UiFileName.Substring(
                              message.DepImageFileNameAr_UiFileName.Length - 5)
                        : message.DepImageFileNameAr_UiFileName,

                DepImageBlobUrlAr = message.DepImageFileNameAr_BlobURL,

                DepImageFileNameEn = message.DepImageFileNameEn,
                DepImageUiFileNameEn =
                    !string.IsNullOrEmpty(message.DepImageFileNameEn_UiFileName) &&
                    message.DepImageFileNameEn_UiFileName.Length > 45
                        ? message.DepImageFileNameEn_UiFileName.Substring(0, 40) +
                          message.DepImageFileNameEn_UiFileName.Substring(
                              message.DepImageFileNameEn_UiFileName.Length - 5)
                        : message.DepImageFileNameEn_UiFileName,

                DepImageBlobUrlEn = message.DepImageFileNameEn_BlobURL,
                SystemModules = new List<SystemModule>()
            };

            var systemModuleTypes = uow
                .GetRepository<SystemModuleType>()
                .GetAllActiveNonDeleted(x =>
                    EvaluationModuleDefaultTypes.Contains(x.BackendName))
                .ToList();

            int orderNo = 1;

            foreach (var item in systemModuleTypes)
            {
                department.SystemModules.Add(new SystemModule
                {
                    NameEn = item.NameEn,
                    NameAr = item.NameAr,
                    OrderNo = orderNo++,
                    DescriptionAr = item.NameAr,
                    DescriptionEn = item.NameEn,
                    SystemModuleTypeId = item.Id,
                    Routing = EvaluationModuleRoutingMap
                    .TryGetValue(item.BackendName, out var routing) ? routing : null,
                    NoDefinition = "EPP_yy",
                    Icon = "fa fa-clipboard",
                    BackendName = department.BackendName + item.BackendName,
                    CreateById = userInfo.UserId ?? Guid.Empty
                });
            }
            uow.GetRepository<Department>().Insert(department);
            foreach (var sm in department.SystemModules)
            {
                Debug.WriteLine($"CreateById = {sm.CreateById}");
            }
            await uow.CommitAsync();

            var result = mapper.Map<DepartmentDTO>(
                department,
                opts => opts.Items["Language"] = _requestInfo.Lang);

            result.ResponseStatus = DBResult.Inserted;

            return result;
        }
        private static readonly Dictionary<string, string> EvaluationModuleRoutingMap =
    new(StringComparer.OrdinalIgnoreCase)
    {
        { SystemModuleTypeBackend.EvaluationParty, SystemModuleRouting.EvaluationParty },
        { SystemModuleTypeBackend.EvaluationPlan, SystemModuleRouting.EvaluationPlan },
        { SystemModuleTypeBackend.EvaluationRequest, SystemModuleRouting.EvaluationRequest }
    };
        private static readonly string[] EvaluationModuleDefaultTypes =
       {
            SystemModuleTypeBackend.EvaluationParty,
            SystemModuleTypeBackend.EvaluationPlan,
            SystemModuleTypeBackend.EvaluationRequest
        };
        public async Task<DepartmentDTO> UpdateDepartment(DepartmentDTO message, List<WebsiteAttachmentDTO>? filemodel)
        {




            var result = new DepartmentDTO();

            if (message.Id is not null)
            {


                Department obj = await uow.GetRepository<Department>()
                                  .GetAllNonDeleted()
                                  .Include(x => x.CreateBy)
                                  .Where(x => x.Id == message.Id)
                                  .FirstAsync();
                var attachmentinserted = 0;
                if (filemodel != null)
                {
                    if (filemodel.Count > 0)
                    {

                        foreach (var item in filemodel)
                        {
                            switch (item.ControlFileName)
                            {
                                case "DepartmentDepImageFileNameAr":
                                    message.DepImageFileNameAr_BlobURL = item.BlobUrl;
                                    message.DepImageFileNameAr = item.FileName;
                                    message.DepImageFileNameAr_UiFileName = item.UiFileName;

                                    break;
                                case "DepartmentDepImageFileNameEn":
                                    message.DepImageFileNameEn_BlobURL = item.BlobUrl;
                                    message.DepImageFileNameEn = item.FileName;
                                    message.DepImageFileNameEn_UiFileName = item.UiFileName;

                                    break;
                                case "WebsiteAttachmentId":
                                    WebsiteAttachment attachment = new WebsiteAttachment();
                                    attachment.FileName = item.FileName;
                                    attachment.UiFileName = item.UiFileName;
                                    attachment.BlobUrl = item.BlobUrl;
                                    attachment.FileExtension = item.FileExtension;
                                    attachment.FileSize = item.FileSize;
                                    attachment.IsActive = true;
                                    attachmentinserted = 1;
                                    uow.GetRepository<WebsiteAttachment>().Insert(attachment);
                                    message.WebsiteAttachmentId = attachment.Id;
                                    break;
                            }
                        }


                    }

                }

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = obj.BackendName;
                obj.RoutingPath = message.RoutingPath;
                obj.DepIcon = message.DepIcon;
                obj.IsEvaluated = message.IsEvaluated;
                //obj.TargetOrgTreeId = message.TargetOrgTreeId;
                //obj.CategoryId = message.CategoryId;
                obj.IsNDA = message.IsNDA;
                obj.DescAr = message.DescAr;
                obj.DescEn = message.DescEn;
                obj.WebsiteAttachmentId = (attachmentinserted == 1 ? message.WebsiteAttachmentId : obj.WebsiteAttachmentId);
                obj.IsActive = message.IsActive;


                if (message.DepImageFileNameAr != null)
                    obj.DepImageFileNameAr = message.DepImageFileNameAr;

                if (message.DepImageFileNameAr_UiFileName != null && message.DepImageFileNameAr_UiFileName.Length > 45)
                {
                    obj.DepImageUiFileNameAr = message.DepImageFileNameAr_UiFileName != null
                        ? message.DepImageFileNameAr_UiFileName.Substring(0, 40) +
                          message.DepImageFileNameAr_UiFileName.Substring(message.DepImageFileNameAr_UiFileName.Length - 5)
                        : null;
                    ;
                }
                else if (message.DepImageFileNameAr_UiFileName != null && message.DepImageFileNameAr_UiFileName.Length < 45)
                {
                    obj.DepImageUiFileNameAr = message.DepImageFileNameAr_UiFileName;

                }

                if (message.DepImageFileNameAr_BlobURL != null)
                    obj.DepImageBlobUrlAr = message.DepImageFileNameAr_BlobURL;

                if (message.DepImageFileNameEn != null)
                    obj.DepImageFileNameEn = message.DepImageFileNameEn;

                if (message.DepImageFileNameEn_UiFileName != null && message.DepImageFileNameEn_UiFileName.Length > 45)
                {
                    obj.DepImageUiFileNameEn = message.DepImageFileNameEn_UiFileName != null
                        ? message.DepImageFileNameEn_UiFileName.Substring(0, 40) +
                          message.DepImageFileNameEn_UiFileName.Substring(message.DepImageFileNameEn_UiFileName.Length - 5)
                        : null;
                    ;
                }
                else if (message.DepImageFileNameEn_UiFileName != null && message.DepImageFileNameEn_UiFileName.Length < 45)
                {
                    obj.DepImageUiFileNameEn = message.DepImageFileNameEn_UiFileName;

                }

                if (message.DepImageFileNameEn_BlobURL != null)
                    obj.DepImageBlobUrlEn = message.DepImageFileNameEn_BlobURL;


                uow.GetRepository<Department>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<DepartmentDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }
        public async Task<bool> UpdateDepartmentOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<Department>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }
        public async Task<DepartmentDTO> DeleteDepartment(Guid? Id)
        {




            var result = new DepartmentDTO();
            if (Id is not null)
            {
                Department obj = await uow.GetRepository<Department>()
                                  .GetAllNonDeleted()
                                  .Where(x => x.Id == Id)
                                  .FirstAsync();
                var SystemModule = await uow.GetRepository<SystemModule>()
 .GetAllNonDeleted()
                       .Where(x => x.DepartmentId == obj.Id)
                       .ToListAsync();
                if (SystemModule.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.DepartmentExistsSystemModule);
                }

                var AcademicYear = await uow.GetRepository<AcademicYear>()
 .GetAllNonDeleted()
                       .Where(x => x.DepartmentId == obj.Id)
                       .ToListAsync();
                if (AcademicYear.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.DepartmentExistsAcademicYear);
                }
                uow.GetRepository<Department>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<DepartmentDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;


        }


        public async Task<List<Department>> GetDepartmentClass()
        {

            var result = await uow.GetRepository<Department>()
               .GetAllNonDeleted()
               .Include(x => x.CreateBy)
               .Distinct()
               .ToListAsync();

            return result;

        }
    }
}