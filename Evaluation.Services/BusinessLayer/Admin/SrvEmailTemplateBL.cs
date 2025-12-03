using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.Template;
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
    public class SrvEmailTemplateBL : AdminBase
    {
        public SrvEmailTemplateBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<EmailTemplateDTO>> GetEmailTemplateList(int Page, int PageSize)
        {
           

            
           

            var list = await uow.GetRepository<EmailTemplate>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<EmailTemplateDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
        public async Task<List<EmailTemplateDocument>> GetEmailTemplateDocument(Guid emailtemplateid)
        {

            var result = await uow.GetRepository<EmailTemplateDocument>()
                    .GetAllNonDeleted()
                    .Include(x=>x.EmailTemplate)
                    .Where(x=>x.EmailTemplateId== emailtemplateid)
                    .ToListAsync();


            return result;
        }
        public async Task<List<DropdownItem>> GetFieldFromService(Guid serviceid)
        {

            var result = await uow.GetRepository<Field>()
                                        .GetAllNonDeleted()
                                        .Include(x => x.FieldType)
                                        .Include(x => x.FormGroup)
                                        .Where(x => x.FieldType!.BackendName == "file" && x.ServiceId==serviceid)
                                        .Select(x => new DropdownItem
                                        {
                                            Id = x.Id,
                                            NameAr = x.FormGroup!.TitleAr + "_"+ x.TitleAr,
                                            NameEn = x.FormGroup.TitleEn + "_" + x.TitleEn
                                        }).ToListAsync();


            return result;
        }
        public async Task<List<DropdownItem>> GetFieldFromEvaluation(Guid serviceid)
        {
            var allservices = await uow.GetRepository<Service>().GetAllNonDeleted().ToListAsync();

            var systemmoduleid=allservices.Where(x=>x.Id==serviceid).Select(x=>x.SystemModuleId).FirstOrDefault();

            var insitalservice=allservices.Where(x=>x.SystemModuleId==systemmoduleid && x.Initialservice==true).Select(x=>x.Id).FirstOrDefault();

            var result =  await uow.GetRepository<Field>()
                                        .GetAllNonDeleted()
                                        .Include(x => x.FieldType)
                                        .Where(x => x.FieldType.BackendName == "file" && x.ServiceId==insitalservice)
                                        .Select(x => new DropdownItem
                                        {
                                            Id = x.Id,
                                            NameAr = x.TitleAr,
                                            NameEn = x.TitleEn
                                        }).ToListAsync();
            return result;
        }
        public async Task<EmailTemplateDTO> SaveEmailTemplate(EmailTemplateDTO message, List<Guid>? EmailTemplateDocument)
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
           
            var BackendName= "EMAIL_TEMPLATE_"+await GenerateBackendNameByTitle(message.TitleEn);
                var existBackendName = await uow
             .GetRepository<EmailTemplate>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

                if (existBackendName != null)
                {

                    message.ResponseStatus = DBResult.BackendExist;
                    return message;
                }

                EmailTemplate obj = new EmailTemplate();

                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.TemplateSubject = message.TemplateSubject;
                obj.TemplateBody = message.TemplateBody;
                obj.BackendName = BackendName;
                obj.EmailProfileId = message.EmailProfileId;
            obj.FielsFromRequest = message.FielsFromRequest != null
    ? string.Join(",", message.FielsFromRequest.Select(g => g.ToString()))
    : string.Empty;
            obj.FielsFromEvaluation = message.FielsFromEvaluation != null
    ? string.Join(",", message.FielsFromEvaluation.Select(g => g.ToString()))
    : string.Empty;
            obj.ServiceId = message.ServiceId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EmailTemplate>().Insert(obj);
                //insert values to EmailtemplateDocument
                if (EmailTemplateDocument!.Count > 0)
                {
                    List<EmailTemplateDocument> objentitylist=new List<EmailTemplateDocument>();
                    foreach (var item in EmailTemplateDocument)
                    {
                        EmailTemplateDocument objentity = new EmailTemplateDocument();
                        objentity.TemplateDocId = item;
                        objentity.EmailTemplateId = obj.Id;
                        objentity.IsActive = true;
                        objentitylist.Add(objentity);
                    }
                    if (objentitylist.Count > 0)
                    {
                        await uow.GetRepository<EmailTemplateDocument>().InsertRange(objentitylist);
                    }


                }
                await uow.CommitAsync();
            var result = mapper.Map<EmailTemplateDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                result.FielsFromRequest = message.FielsFromRequest;
                result.FielsFromEvaluation = message.FielsFromEvaluation;
                return result;
           
           
            
        }
        public async Task<EmailTemplateDTO> UpdateEmailTemplate(EmailTemplateDTO message, List<Guid>?EmailTemplateDocument)
        {
           
            
          
               
                var result = new EmailTemplateDTO();

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
               
                EmailTemplate obj = await uow.GetRepository<EmailTemplate>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.TemplateSubject = message.TemplateSubject;
                obj.TemplateBody = message.TemplateBody;
                obj.BackendName = obj.BackendName;
                obj.EmailProfileId = message.EmailProfileId;
                obj.FielsFromRequest = message.FielsFromRequest != null
   ? string.Join(",", message.FielsFromRequest.Select(g => g.ToString()))
   : string.Empty;
                obj.FielsFromEvaluation = message.FielsFromEvaluation != null
        ? string.Join(",", message.FielsFromEvaluation.Select(g => g.ToString()))
        : string.Empty;
                obj.ServiceId = message.ServiceId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EmailTemplate>().Update(obj);

                //update values to EmailTemplateDocument
                List<EmailTemplateDocument>  objEmailTemplateDocumententitydelete = await uow.GetRepository<EmailTemplateDocument>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.EmailTemplateId == obj.Id)
                                      .ToListAsync();

                var EmailTemplateDocumentexistids =new List<Guid>();
                if (objEmailTemplateDocumententitydelete.Count > 0)
                {
                    foreach (var item in objEmailTemplateDocumententitydelete)
                    {
                        if (EmailTemplateDocument!.Contains(item.TemplateDocId))
                        {
                            EmailTemplateDocumentexistids.Add(item.TemplateDocId);
                        }
                        else
                        {
                            uow.GetRepository<EmailTemplateDocument>().Delete(item);
                        }

                    }
                }
                if (EmailTemplateDocument!.Count > 0)
                {
                    var notInSelected = EmailTemplateDocument.Except(EmailTemplateDocumentexistids).ToList();
                    List<EmailTemplateDocument> objentitylist=new List<EmailTemplateDocument>();
                    foreach (var item in notInSelected)
                    {
                        EmailTemplateDocument objentity = new EmailTemplateDocument();
                        objentity.EmailTemplateId = obj.Id;
                        objentity.TemplateDocId = item;
                        objentity.IsActive = true;
                        objentitylist.Add(objentity);
                    }
                    if (objentitylist.Count > 0)
                    {
                        await uow.GetRepository<EmailTemplateDocument>().InsertRange(objentitylist);
                    }


                }
                await uow.CommitAsync();
                result = mapper.Map<EmailTemplateDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
                result.FielsFromRequest = message.FielsFromRequest;
                result.FielsFromEvaluation = message.FielsFromEvaluation;
            }

                return result;
           
        }
        
        public async Task<EmailTemplateDTO> DeleteEmailTemplate(Guid? Id)
        {

           

                
                var result = new EmailTemplateDTO();
                if (Id is not null)
                {
                    EmailTemplate obj = await uow.GetRepository<EmailTemplate>()
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
                      .Where(x => x.EmailTemplateId == obj.Id)
                      .ToListAsync();
                if (EmailTemplateDocument.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.EmailTemplateExistsEmailTemplateDocument);
                }
                var ActionStatusConfigNotification = await uow.GetRepository<ActionStatusConfigNotification>()
.GetAllNonDeleted()
                      .Where(x => x.EmailTemplateId == obj.Id)
                      .ToListAsync();
                if (ActionStatusConfigNotification.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.EmailTemplateExistsActionStatusConfigNotification);
                }
                uow.GetRepository<EmailTemplate>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<EmailTemplateDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
