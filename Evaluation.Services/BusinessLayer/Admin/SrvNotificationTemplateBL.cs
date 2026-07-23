using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
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
    public class SrvNotificationTemplateBL : AdminBase
    {
        public SrvNotificationTemplateBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<NotificationTemplateDTO>> GetNotificationTemplateList(Guid? systemmoduleid, int Page, int PageSize)
        {
           

            
           
            //systemmoduleid = systemmoduleid == "-1" ? null : systemmoduleid;
            var list = await uow.GetRepository<NotificationTemplate>()
                .GetAllNonDeleted()
                .Where(x=>systemmoduleid != Guid.Empty?systemmoduleid==x.SystemModuleId:x.SystemModuleId==x.SystemModuleId)
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<NotificationTemplateDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
   
        public async Task<NotificationTemplateDTO> SaveNotificationTemplate(NotificationTemplateDTO message)
        {
           
           

            var BackendName= "NOTIFICATION_TEMPLATE"+"_"+await GenerateBackendNameByTitle(message.SubjectEn??string.Empty);
            var existBackendName = await uow
             .GetRepository<NotificationTemplate>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {

                message.ResponseStatus = DBResult.BackendExist;
                return message;
            }

            NotificationTemplate obj = new NotificationTemplate();

            obj.SubjectAr = message.SubjectAr;
            obj.SubjectEn = message.SubjectEn;
            obj.BodyAr = message.BodyAr;
            obj.BodyEn = message.BodyEn;
            obj.SystemModuleId = message.SystemModuleId;
            obj.BackendName = BackendName;
            obj.IsActive = message.IsActive;
            obj.DepartmentId = message.DepartmentId;
            obj.ServiceId = message.ServiceId;

            uow.GetRepository<NotificationTemplate>().Insert(obj);
                await uow.CommitAsync();
            var result = mapper.Map<NotificationTemplateDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
            
        }
        public async Task<NotificationTemplateDTO> UpdateNotificationTemplate(NotificationTemplateDTO message)
        {
           
            
          
               
                var result = new NotificationTemplateDTO();

                if (message.Id is not null)
                {
                    

                    NotificationTemplate obj = await uow.GetRepository<NotificationTemplate>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.SubjectAr = message.SubjectAr;
                obj.SubjectEn = message.SubjectEn;
                obj.BodyAr = message.BodyAr;
                obj.BodyEn = message.BodyEn;
                obj.SystemModuleId = message.SystemModuleId;
                obj.BackendName = obj.BackendName;
                obj.DepartmentId = obj.DepartmentId;
                obj.ServiceId = obj.ServiceId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<NotificationTemplate>().Update(obj);
                    await uow.CommitAsync();
                result = mapper.Map<NotificationTemplateDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
                }

                return result;
           
        }
       
        public async Task<NotificationTemplateDTO> DeleteNotificationTemplate(Guid? Id)
        {

           

               
                var result = new NotificationTemplateDTO();
                if (Id is not null)
                {
                    NotificationTemplate obj = await uow.GetRepository<NotificationTemplate>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                var ActionStatusConfigNotification = await uow.GetRepository<ActionStatusConfigNotification>()
.GetAllNonDeleted()
                      .Where(x => x.NotificationTemplateId == obj.Id)
                      .ToListAsync();
                if (ActionStatusConfigNotification.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.NotoficationTemplateExistsActionStatusConfigNotification);
                }
                uow.GetRepository<NotificationTemplate>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<NotificationTemplateDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
