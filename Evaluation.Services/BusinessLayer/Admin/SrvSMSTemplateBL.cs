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
    public class SrvSMSTemplateBL : AdminBase
    {
        public SrvSMSTemplateBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<SMSTemplateDTO>> GetSMSTemplateList(int Page, int PageSize)
        {





            var list = await uow.GetRepository<SMSTemplate>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<SMSTemplateDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }

        public async Task<SMSTemplateDTO> SaveSMSTemplate(SMSTemplateDTO message)
        {



            var BackendName = "SMS_TEMPLATE" + "_" + await GenerateBackendNameByTitle(message.TitleEn);
            var existBackendName = await uow
             .GetRepository<SMSTemplate>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {

                message.ResponseStatus = DBResult.BackendExist;
                return message;
            }

            SMSTemplate obj = new SMSTemplate();

            obj.TitleAr = message.TitleAr;
            obj.TitleEn = message.TitleEn;
            obj.Messages = message.Messages;
            obj.BackendName = BackendName;
            obj.SMSProfileId = message.SMSProfileId;
            obj.SMSProfile = null;
            obj.IsActive = message.IsActive;
            obj.ServiceId = message.ServiceId;
            obj.DepartmentId = message.DepartmentId;

            uow.GetRepository<SMSTemplate>().Insert(obj);
            //insert values to SMSTemplateDocument

            await uow.CommitAsync();
            var result = mapper.Map<SMSTemplateDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;

        }
        public async Task<SMSTemplateDTO> UpdateSMSTemplate(SMSTemplateDTO message)
        {




            var result = new SMSTemplateDTO();

            if (message.Id is not null)
            {


                SMSTemplate obj = await uow.GetRepository<SMSTemplate>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.Messages = message.Messages;
                obj.BackendName = obj.BackendName;
                obj.SMSProfileId = message.SMSProfileId;
                obj.IsActive = message.IsActive;
                obj.DepartmentId = message.DepartmentId;
                obj.ServiceId = message.ServiceId;
                uow.GetRepository<SMSTemplate>().Update(obj);


                await uow.CommitAsync();
                result = mapper.Map<SMSTemplateDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

        public async Task<SMSTemplateDTO> DeleteSMSTemplate(Guid? Id)
        {




            var result = new SMSTemplateDTO();
            if (Id is not null)
            {
                SMSTemplate obj = await uow.GetRepository<SMSTemplate>()
                                  .GetAllNonDeleted()
                                  .Where(x => x.Id == Id)
                                  .FirstAsync();
                var ActionStatusConfigNotification = await uow.GetRepository<ActionStatusConfigNotification>()
.GetAllNonDeleted()
                      .Where(x => x.SMSTemplateId == obj.Id)
                      .ToListAsync();
                if (ActionStatusConfigNotification.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SmsTemplateExistsActionStatusConfigNotification);
                }
                uow.GetRepository<SMSTemplate>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<SMSTemplateDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;


        }

    }
}
