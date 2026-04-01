using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Models.ActionEntities;
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
    public class SrvTemplateDocumentBL : AdminBase
    {
        private readonly AzureBlobStorageService _blobService;
        public SrvTemplateDocumentBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo, AzureBlobStorageService blobService) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            _blobService=blobService;
        }

        public async Task<List<TempLateDocDTO>> GetTemplateDocList(int Page, int PageSize,Guid serviceId)
        {
          
           

            var list = await uow.GetRepository<TemplateDocument>()
                .GetAllNonDeleted()
                .Include(x => x.Service)
                .Include(x => x.CreateBy)
                .Where(x=>x.ServiceId==serviceId)
                .OrderByDescending(x=>x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<TempLateDocDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;
            
        }

        public async Task<WebsiteAttachment> GetAttachment(Guid Id)
        {



            var result = await uow.GetRepository<WebsiteAttachment>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstOrDefaultAsync();
            return result;


        }

        public async Task<TempLateDocDTO> SaveTemplateDoc(TempLateDocDTO message, List<WebsiteAttachmentDTO>? filemodel)
        {
           
           
            if (message.ServiceId is not null)
            {
                var isfreezcount = await uow.GetRepository<Service>()
                                          .GetAllNonDeleted(x => x.Id == message.ServiceId && x.IsFreez == true).ToListAsync();
                if (isfreezcount.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_ADD);

                }
            }
           
            if (message.IsAttachment == true && filemodel!=null)
            {
                if (filemodel.Count > 0)
                {

                    foreach (var item in filemodel)
                    {
                        WebsiteAttachment attachment = new WebsiteAttachment();
                        attachment.FileName = item.FileName;
                        attachment.UiFileName = item.UiFileName;
                        attachment.BlobUrl = item.BlobUrl;
                        attachment.FileExtension = item.FileExtension;
                        attachment.FileSize = item.FileSize;
                        attachment.IsActive = true;
                        await uow.GetRepository<WebsiteAttachment>().InsertAsync(attachment);
                        message.AttachmentId = attachment.Id;
                    }


                }
               
            }

            TemplateDocument obj = new TemplateDocument();

                obj.NameEn = message.NameEn;
                obj.NameAr = message.NameAr;
                obj.TemplateAr = message.TemplateAr;
                obj.TemplateEn = message.TemplateEn;
                obj.AttachmentId = message.AttachmentId;
                obj.IsAttachment = message.IsAttachment;
                obj.DepartmentId = message.DepartmentId;
                obj.ServiceId = message.ServiceId!.Value;
                obj.TemplateGenrationTypeId = message.TemplateGenrationTypeId;
                obj.IsActive = message.IsActive;

               await uow.GetRepository<TemplateDocument>().InsertAsync(obj);
                await uow.CommitAsync();
            var result = mapper.Map<TempLateDocDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.UpdateBy = userInfo.DBName;
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
        }
        public async Task<TempLateDocDTO> UpdateTemplateDoc(TempLateDocDTO message, List<WebsiteAttachmentDTO>? filemodel)
        {
           
            
           
               
                var result = new TempLateDocDTO();

                if (message.Id is not null)
                {
                if (message.ServiceId is not null)
                {
                    var isfreezcount = await uow.GetRepository<Service>()
                                          .GetAllNonDeleted(x => x.Id == message.ServiceId && x.IsFreez == true).ToListAsync();
                    if (isfreezcount.Count > 0)
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_EDIT);

                    }
                }
               
                var attachmentinserted = 0;
                if (message.IsAttachment == true && filemodel!=null)
                {
                    if (filemodel.Count > 0)
                    {

                        foreach (var item in filemodel)
                        {
                            WebsiteAttachment attachment = new WebsiteAttachment();
                            attachment.FileName = item.FileName;
                            attachment.UiFileName = item.UiFileName;
                            attachment.BlobUrl = item.BlobUrl;
                            attachment.FileExtension = item.FileExtension;
                            attachment.FileSize = item.FileSize;
                            attachment.IsActive = true;
                            attachmentinserted = 1;
                            uow.GetRepository<WebsiteAttachment>().Insert(attachment);
                            message.AttachmentId = attachment.Id;
                        }


                    }
                    
                }


                TemplateDocument obj = await uow.GetRepository<TemplateDocument>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameEn = message.NameEn;
                obj.NameAr = message.NameAr;
                obj.TemplateAr = message.TemplateAr;
                obj.TemplateEn = message.TemplateEn;
                obj.AttachmentId = (message.IsAttachment == true ? ((attachmentinserted == 1) ? message.AttachmentId : obj.AttachmentId) : null);
                obj.IsAttachment = message.IsAttachment;
                obj.DepartmentId = message.DepartmentId;
                obj.ServiceId = message.ServiceId!.Value;
                obj.TemplateGenrationTypeId = message.TemplateGenrationTypeId;
                obj.IsActive = message.IsActive;
                uow.GetRepository<TemplateDocument>().Update(obj);
                    await uow.CommitAsync();
                result = mapper.Map<TempLateDocDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
                }

                return result;
           
        }
        public async Task<TempLateDocDTO> DeleteTemplateDoc(Guid? Id)
        {

           
                
                var result = new TempLateDocDTO();
                if (Id is not null)
                {
                TemplateDocument obj = await uow.GetRepository<TemplateDocument>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                var isfreezcount = await uow.GetRepository<Service>()
                                          .GetAllNonDeleted(x => x.Id == obj.ServiceId && x.IsFreez == true).ToListAsync();
                if (isfreezcount.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_DELETE);

                }
                var EmailTemplateDocument = await uow.GetRepository<EmailTemplateDocument>()
.GetAllNonDeleted()
                      .Where(x => x.TemplateDocId == obj.Id)
                      .ToListAsync();
                if (EmailTemplateDocument.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.TemplateDocExistsEmailTemplateDocument);
                }
                var ActionTemplateDoc = await uow.GetRepository<ActionTemplateDoc>()
.GetAllNonDeleted()
                      .Where(x => x.TemplateDocId == obj.Id)
                      .ToListAsync();
                if (ActionTemplateDoc.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.TemplateDocExistsActionTemplateDoc);
                }
                uow.GetRepository<TemplateDocument>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<TempLateDocDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
