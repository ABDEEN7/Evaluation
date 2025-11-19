using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.StatusEntities;
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
    public class SrvActionStatusConfigurationBL : AdminBase
    {
        private readonly CacheDataProvider _CacheDataProvider;
        public SrvActionStatusConfigurationBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo, CacheDataProvider CacheDataProvider) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            _CacheDataProvider = CacheDataProvider;
        }

        #region Action Status Configuration
        
        public async Task<List<ActionStatusConfigurationDTO>> GetActionStatusConfigurationList(AdminSearchDTO message)
        {

           

            var list = await uow.GetRepository<ActionStatusConfiguration>()
                .GetAllNonDeleted()
                .Include(x => x.ServiceAction)
                .Include(x => x.CurrentStatus)
                .Include(x => x.NextStatus)
                .Include(x => x.CreateBy)
                .Where(x=>x.ServiceAction!.ServiceId==message.ServiceId)
                .OrderBy(x=>x.OrderNo)
                .ThenByDescending(x=>x.CreateDate)
                .ToListAsync();
            if (message.serviceActionId is not null)
            {
                list = list.Where(x =>x.ServiceActionId== message.serviceActionId
                ).ToList();
            }
            if (message.currentStatusId is not null)
            {
                list = list.Where(c => c.CurrentStatusId == message.currentStatusId).ToList();
            }
            if (message.nextStatusId is not null)
            {
                list = list.Where(c => c.NextStatusId == message.nextStatusId).ToList();
            }
            list = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();
            var result = mapper.Map<List<ActionStatusConfigurationDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }
        public async Task<List<ActionStatusConfigurationDTO>> GetActionStatusConfigurationByActionList(Guid ServiceId, Guid actionId)
        {

           
            var list = await uow.GetRepository<ActionStatusConfiguration>()
                .GetAllNonDeleted()
                .Include(x => x.ServiceAction)
                .Include(x => x.CurrentStatus)
                .Include(x => x.NextStatus)
                .Include(x => x.CreateBy)
                .Where(x=>x.ServiceAction!.ServiceId== ServiceId && x.ServiceActionId== actionId)
                 .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                .ToListAsync();

            var result = mapper.Map<List<ActionStatusConfigurationDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }
        public async Task<List<ActionStatusConfigurationDTO>> GetActionStatusConfigurationByStatusList(Guid ServiceId, Guid statusIdId)
        {

          
            var list = await uow.GetRepository<ActionStatusConfiguration>()
                .GetAllNonDeleted()
                .Include(x => x.ServiceAction)
                .Include(x => x.CurrentStatus)
                .Include(x => x.NextStatus)
                .Include(x => x.CreateBy)
                .Where(x=>x.ServiceAction!.ServiceId==ServiceId && x.CurrentStatusId==statusIdId)
                 .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                .ToListAsync();

            var result = mapper.Map<List<ActionStatusConfigurationDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }
        public async Task<List<ActionDTO>> GetAllActionList(Guid ServiceId)
        {

            
            var result = await uow.GetRepository<ServiceAction>()
                .GetAllNonDeleted()
                .Include(x => x.ActionType)
                .Include(x => x.CreateBy)
                .Where(x=>x.ServiceId==ServiceId)
                .OrderByDescending(x => x.CreateDate)
                .Select(x=>new ActionDTO
                {
                    Id=x.Id,
                    BakendName=x.ActionType!.BackendName
                })
                .ToListAsync();

          

            return result;

        }
        public async Task<List<ActionDTO>> GetActionList(Guid ServiceId)
        {


            var result = await uow.GetRepository<ServiceAction>()
                .GetAllNonDeleted()
                .Where(x=>x.ServiceId==ServiceId)
                .OrderByDescending(x => x.CreateDate)
                .Select(x=>new ActionDTO
                {
                    Id=x.Id,
                    NameAr=x.NameAr,
                    NameEn=x.NameEn
                    
                })
                .ToListAsync();



            return result;

        }
        public async Task<List<ActionDTO>> GetStatusList(Guid ServiceId)
        {


            var result = await uow.GetRepository<ServiceStatus>()
                .GetAllNonDeleted()
                .Where(x=>x.ServiceId==ServiceId)
                .OrderByDescending(x => x.CreateDate)
                .Select(x=>new ActionDTO
                {
                    Id=x.Id,
                    NameAr=x.NameAr,
                    NameEn=x.NameEn

                })
                .ToListAsync();



            return result;

        }
        public async Task<ActionStatusConfigurationDTO> SaveActionStatusConfiguration(ActionStatusConfigurationDTO message)
        {

          
            if (message.ShowIsDefaultAssigner)
            {
                var actiontype=await uow.GetRepository<ServiceAction>().GetAllNonDeleted()
                    .Include(x=>x.ActionType).Where(x=>x.Id==message.ServiceActionId).FirstOrDefaultAsync();
                if (actiontype != null)
                {
                    if (actiontype.ActionType!.BackendName != "ASSIGN" && actiontype.ActionType.BackendName != "APPROVE_AND_ASSIGN")
                    {
                        message.ShowIsDefaultAssigner = false;
                    }

                }
            }


            ActionStatusConfiguration obj = new ActionStatusConfiguration();

                obj.ServiceActionId = message.ServiceActionId;
                obj.CurrentStatusId = message.CurrentStatusId;
                obj.NextStatusId = message.NextStatusId;
                obj.IsRemark = message.IsRemark;
                obj.RemarkLabelAr = message.RemarkLabelAr;
                obj.RemarkLabelEn = message.RemarkLabelEn;
                obj.IsRemarkRequired = message.IsRemarkRequired;
                obj.IsOtherAttachment = message.IsOtherAttachment;
                obj.AttachmentLabelAr = message.AttachmentLabelAr;
                obj.AttachmentLabelEn = message.AttachmentLabelEn;
                obj.IsOtherAttachmentRequired = message.IsOtherAttachmentRequired;
                obj.ShowIsDefaultAssigner = message.ShowIsDefaultAssigner;
                obj.IsAuto = message.IsAuto;
                obj.IsActive = message.IsActive;
                uow.GetRepository<ActionStatusConfiguration>().Insert(obj);
                await uow.CommitAsync();
            var result = mapper.Map<ActionStatusConfigurationDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            result.ServiceId= await uow.GetRepository<ServiceAction>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == obj.ServiceActionId)
                                      .Select(x => x.ServiceId)
                                      .FirstAsync();
           // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_ACTIONSTATUSCONFIG);
            return result;
           
        }

        public async Task<ActionStatusConfigurationDTO> UpdateActionStatusConfiguration(
            ActionStatusConfigurationDTO message)
        {



           
            var result = new ActionStatusConfigurationDTO();



            ActionStatusConfiguration obj = await uow.GetRepository<ActionStatusConfiguration>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .Where(x => x.Id == message.Id)
                .FirstAsync();

            if (message.ShowIsDefaultAssigner)
            {
                var actiontype = await uow.GetRepository<ServiceAction>().GetAllNonDeleted()
                    .Include(x => x.ActionType).Where(x => x.Id == message.ServiceActionId).FirstOrDefaultAsync();
                if (actiontype != null)
                {
                    if (actiontype.ActionType!.BackendName != "ASSIGN" && actiontype.ActionType.BackendName != "APPROVE_AND_ASSIGN")
                    {
                        message.ShowIsDefaultAssigner = false;
                    }

                }
            }

            obj.ServiceActionId = message.ServiceActionId;
            obj.CurrentStatusId = message.CurrentStatusId;
            obj.NextStatusId = message.NextStatusId;
            obj.IsRemark = message.IsRemark;
            obj.RemarkLabelAr = message.RemarkLabelAr;
            obj.RemarkLabelEn = message.RemarkLabelEn;
            obj.IsRemarkRequired = message.IsRemarkRequired;
            obj.IsOtherAttachment = message.IsOtherAttachment;
            obj.AttachmentLabelAr = message.AttachmentLabelAr;
            obj.AttachmentLabelEn = message.AttachmentLabelEn;
            obj.IsOtherAttachmentRequired = message.IsOtherAttachmentRequired;
            obj.ShowIsDefaultAssigner = message.ShowIsDefaultAssigner;
            obj.IsAuto = message.IsAuto;
            obj.IsActive = message.IsActive;
            uow.GetRepository<ActionStatusConfiguration>().Update(obj);
            await uow.CommitAsync();
             result = mapper.Map<ActionStatusConfigurationDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Updated;
            result.ServiceId = await uow.GetRepository<ServiceAction>()
                .GetAllNonDeleted()
                .Where(x => x.Id == obj.ServiceActionId)
                .Select(x => x.ServiceId)
                .FirstAsync();
           // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_ACTIONSTATUSCONFIG);

            return result;

        }

        public async Task<bool> UpdateActionStatusConfigurationOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<ActionStatusConfiguration>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }

        public async Task<ActionStatusConfigurationDTO> DeleteActionStatusConfiguration(Guid Id)
        {



           
            var result = new ActionStatusConfigurationDTO();
            ActionStatusConfiguration obj = await uow.GetRepository<ActionStatusConfiguration>()
                .GetAllNonDeleted()
                .Where(x => x.Id == Id)
                .FirstAsync();
            var ActionStatusConfigNotification = await uow.GetRepository<ActionStatusConfigNotification>()
                .GetAllNonDeleted()
                                      .Where(x => x.ActionStatusConfigurationId == obj.Id)
                                      .ToListAsync();
            if (ActionStatusConfigNotification.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.ExistsActionStatusConfigNotification);
            }
            uow.GetRepository<ActionStatusConfiguration>().Delete(obj);
            await uow.CommitAsync();
             result = mapper.Map<ActionStatusConfigurationDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Deleted;
            //await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_ACTIONSTATUSCONFIG);
            return result;

        }

        #endregion
        #region Action Status Configuration Notification
        public async Task<List<ActionStatusConfigNotificationDTO>> GetActionStatusConfigurationNotificationList(Guid actionstatusconfigid)
        {

           
            var list = await uow.GetRepository<ActionStatusConfigNotification>()
                .GetAllNonDeleted()
                .Include(x => x.PartyType)
                .Include(x => x.CreateBy)
                .Where(x=>x.ActionStatusConfigurationId==actionstatusconfigid)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var result = mapper.Map<List<ActionStatusConfigNotificationDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }

        public async Task<ActionStatusConfigNotificationDTO> SaveActionStatusConfigurationNotification(ActionStatusConfigNotificationDTO message)
        {

            
            
            ActionStatusConfigNotification obj = new ActionStatusConfigNotification();

            obj.ActionStatusConfigurationId = message.ActionStatusConfigurationId;
            obj.PartyTypeId = message.PartyTypeId;
            obj.IsEmailSend = message.IsEmailSend;
            obj.EmailTemplateId = message.EmailTemplateId.HasValue? message.EmailTemplateId:null;
            obj.IsMessageSend = message.IsMessageSend;
            obj.SMSTemplateId = message.SMSTemplateId.HasValue ? message.SMSTemplateId : null;
            obj.IsNotificationSend = message.IsNotificationSend;
            obj.NotificationTemplateId = message.NotificationTemplateId.HasValue ? message.NotificationTemplateId : null;
            obj.IsActive = message.IsActive;
            uow.GetRepository<ActionStatusConfigNotification>().Insert(obj);
            await uow.CommitAsync();
            var result = mapper.Map<ActionStatusConfigNotificationDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;

        }

        public async Task<ActionStatusConfigNotificationDTO> UpdateActionStatusConfigurationNotification(
            ActionStatusConfigNotificationDTO message)
        {



            
            var result = new ActionStatusConfigNotificationDTO();


            ActionStatusConfigNotification obj = await uow.GetRepository<ActionStatusConfigNotification>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .Where(x => x.Id == message.Id)
                .FirstAsync();

            obj.ActionStatusConfigurationId = message.ActionStatusConfigurationId;
            obj.PartyTypeId = message.PartyTypeId;
            obj.IsEmailSend = message.IsEmailSend;
            obj.EmailTemplateId = message.EmailTemplateId.HasValue ? message.EmailTemplateId : null;
            obj.IsMessageSend = message.IsMessageSend;
            obj.SMSTemplateId = message.SMSTemplateId.HasValue ? message.SMSTemplateId : null;
            obj.IsNotificationSend = message.IsNotificationSend;
            obj.NotificationTemplateId = message.NotificationTemplateId.HasValue ? message.NotificationTemplateId : null;
            obj.IsActive = message.IsActive;
            uow.GetRepository<ActionStatusConfigNotification>().Update(obj);
            await uow.CommitAsync();
             result = mapper.Map<ActionStatusConfigNotificationDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Updated;


            return result;

        }

        public async Task<ActionStatusConfigNotificationDTO> DeleteActionStatusConfigurationNotification(Guid Id)
        {



           
            var result = new ActionStatusConfigNotificationDTO();
            ActionStatusConfigNotification obj = await uow.GetRepository<ActionStatusConfigNotification>()
                .GetAllNonDeleted()
                .Where(x => x.Id == Id)
                .FirstAsync();
            uow.GetRepository<ActionStatusConfigNotification>().Delete(obj);
            await uow.CommitAsync();
             result = mapper.Map<ActionStatusConfigNotificationDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Deleted;

            return result;

        }

        #endregion

    }
}
